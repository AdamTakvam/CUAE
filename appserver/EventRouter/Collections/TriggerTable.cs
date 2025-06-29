using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Core.ConfigData;

using Metreos.ApplicationFramework;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.AppServer.EventRouter.Collections
{
	internal sealed class TriggerTable : IEnumerable
	{
        // Event name -> StringCollection of trigger IDs
        private Hashtable triggerTable;

        // Trigger ID -> TriggerInfo
        private Hashtable triggerInfoTable;

        public int Count { get { return triggerInfoTable.Count; } }

        public TriggerInfo this[string triggerId]
        {
            get { return triggerInfoTable[triggerId] as TriggerInfo; }
        }

		public TriggerTable()
		{
            triggerTable = new Hashtable();
            triggerInfoTable = new Hashtable();
		}
        
        public string GetId(string applicationName, string scriptName, string partitionName)
        {
            IDictionaryEnumerator dictEnum = triggerInfoTable.GetEnumerator();
            while(dictEnum.MoveNext())
            {
                string id = dictEnum.Key as string;
                TriggerInfo info = dictEnum.Value as TriggerInfo;

                Assertion.Check(info.ID == id, "TriggerInfo table inconsistency detected.");

                if(info.appName == applicationName && 
                    info.scriptName == scriptName &&
                    info.partitionName == partitionName)
                {
                    return id;
                }
            }

            return null;
        }

        public string[] GetIds(string applicationName, string scriptName)
        {
            StringCollection ids = new StringCollection();

            IDictionaryEnumerator dictEnum = triggerInfoTable.GetEnumerator();
            while(dictEnum.MoveNext())
            {
                string id = dictEnum.Key as string;
                TriggerInfo info = dictEnum.Value as TriggerInfo;

                Assertion.Check(info.ID == id, "TriggerInfo table inconsistency detected.");

                if(info.appName == applicationName && 
                    info.scriptName == scriptName)
                {
                    ids.Add(id);
                }
            }

            if(ids.Count == 0)
            {
                return null;
            }
            else
            {
                string[] idsArray = new string[ids.Count];
                ids.CopyTo(idsArray, 0);
                return idsArray;
            }
        }

        public string Add(TriggerInfo trigger)
        {
            string triggerId = System.Guid.NewGuid().ToString();

            StringCollection sc = triggerTable[trigger.eventName] as StringCollection;

            if(sc == null)
            {
                sc = new StringCollection();
                triggerTable[trigger.eventName] = sc;
            }

            sc.Add(triggerId);

            triggerInfoTable.Add(triggerId, trigger);

            return triggerId;
        }

        public void Remove(StringCollection ids)
        {
            if(ids == null) { return; }

            foreach(string triggerId in ids)
            {
                Remove(triggerId);
            }
        }

        public void Remove(string triggerId)
        {
            if(triggerId == null) { return; }

            triggerInfoTable.Remove(triggerId);
        }

        public TriggerInfo GetTriggerInfo(EventMessage incomingEvent)
        {
            string eventName = incomingEvent.MessageId;

            StringCollection ids = triggerTable[eventName] as StringCollection;
            if(ids == null) { return null; }

            StringCollection defunctIds = new StringCollection();

            TriggerInfo matchingTriggerInfo = null;
            foreach(string currId in ids)
            {
                TriggerInfo tInfo = triggerInfoTable[currId] as TriggerInfo;
                
                if(tInfo == null)
                {
                    defunctIds.Add(currId);
                    continue;
                }

                // Skip disabled triggers
                if(tInfo.enabled == false) { continue; }
                Assertion.Check(tInfo.eventName == eventName, "Data integrity error in trigger table");

                bool match = EventCollection.MatchEventParams(incomingEvent, tInfo.eventParams);

                if((match == true) && (matchingTriggerInfo == null))
                {
                    matchingTriggerInfo = tInfo;
                }
                else if((match == true) && (tInfo.eventParams.Count > matchingTriggerInfo.eventParams.Count))
                {
                    matchingTriggerInfo = tInfo;
                }
            }

            // Remove defunct trigger IDs
            for(int i=0; i<defunctIds.Count; i++)
            {
                ids.Remove(defunctIds[i]);
            }

            return matchingTriggerInfo;
        }

        public void Clear()
        {
            triggerTable.Clear();
            triggerInfoTable.Clear();
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return this.triggerInfoTable.Values.GetEnumerator();
        }

        #endregion
    }
}
