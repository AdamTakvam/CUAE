using System;
using System.Threading;
using System.Globalization;

namespace Metreos.Core.ConfigData
{
    public sealed class TriggerInfo
    {
        public string ID;
        public EventParamCollection eventParams;
        public string eventName;
        public string appName;
        public string scriptName;
        public string sessionGuid;

        public string partitionName;
        public CultureInfo culture;
        public bool enabled;
        
        private int numHits;
        public int NumHits 
        { 
            set { numHits = value; } 
        }

        public bool Expired { get { return numHits == 0; } }

        public bool IsMaster 
        { 
            get { return sessionGuid == null ? true : false; } 
        }

        public bool IsComplete 
        { 
            get { return ((eventName != null) && (appName != null) && (scriptName != null)) ? true : false; } 
        }

        public TriggerInfo()
            : this(null) {}

		public TriggerInfo(string ID)
		{
            this.ID = ID;
            
            this.enabled = true;
            this.numHits = -1;
            this.eventParams = new EventParamCollection();
		}

        public void DecrementNumHits()
        {
            if(IsMaster) { return; }

            if(numHits > 0)
            {
                numHits--;
            }
        }

        public void Clear()
        {
            if(eventParams != null)
            {
                eventParams.Clear();
                eventParams = null;
            }

            ID = null;
            eventName = null;
            appName = null;
            scriptName = null;
            sessionGuid = null;
            numHits = 0;
            enabled = false;
        }
	}
}
