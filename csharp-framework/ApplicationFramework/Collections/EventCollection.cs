using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.ApplicationFramework.Assembler;

namespace Metreos.ApplicationFramework.Collections
{
    /// <summary>
    /// Collection of EventInfo objects.
    /// Supports multiple parameters with the same name.
    /// Index by name.
    /// </summary>
    public class EventCollection : IEnumerable
    {
        public const string REGEX_TOKEN  = "regex:";

        private ArrayList events;
        private bool gotTrigger = false;

        public EventCollection()
        {
            events = new ArrayList();
        }

        public void Add(EventInfo eInfo)
        {
            if(eInfo.type == EventInfo.Type.Triggering)
            {
                if(gotTrigger)
                {
                    throw new TriggerException("A script cannot have multiple triggers");
                }
                else
                {
                    gotTrigger = true;
                }
            }

            events.Add(eInfo);
        }

        public EventInfo GetTriggeringEvent()
        {
            if(gotTrigger == false) { return null; }

            EventInfo eInfo = null;
            for(int i=0; i<events.Count; i++)
            {
                eInfo = events[i] as EventInfo;
                
                Debug.Assert(eInfo != null, "Encountered null EventInfo object in event collection");

                if(eInfo.type == EventInfo.Type.Triggering)
                {
                    return eInfo;
                }
            }

            return null;
        }

        public static bool MatchEventParams(EventMessage incomingEvent, EventParamCollection triggerParams)
        {
            foreach(EventParam eParam in triggerParams)
            {
                if(eParam.Value == null) { return false; }

                object inParamObj = null;
                if(String.Compare(eParam.name, ICommands.Fields.USER_DATA, true) == 0)
                {
                    inParamObj = incomingEvent.UserData;
                }
                else
                {
                    inParamObj = incomingEvent[eParam.name];
                }

                if(inParamObj == null) { return false; }

                string inParamValue = inParamObj.ToString();

                if(eParam.Value is StringCollection)
                {
                    // See if the incoming value is contained in the param value collection
                    bool found = false;
                    IEnumerable reqValueEnum = eParam.Value as IEnumerable;
                    foreach(object reqValue in reqValueEnum)
                    {
                        // Perform literal case-insensitive string compare
                        if(String.Compare(inParamValue, reqValue.ToString(), true) == 0)
                        {
                            found = true;
                            break;
                        }
                    }

                    if(found == false)
                    {
                        return false;
                    }
                }
                else
                {
                    string eParamStr = eParam.Value.ToString();
                    if(eParamStr == null) { continue; }

                    if(eParamStr.StartsWith(REGEX_TOKEN))
                    {
                        eParamStr = eParamStr.Substring(REGEX_TOKEN.Length);
                        if(Regex.IsMatch(inParamValue, eParamStr, RegexOptions.Singleline) == false)
                        {
                            return false;
                        }
                    }
                    // Perform literal case-insensitive string compare
                    else if(String.Compare(inParamValue, eParamStr, true) != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public string GetHandler(EventMessage incomingEvent)
        {
            ArrayList matchingHandlers = new ArrayList();

            int mostNumMatchingParams = -1;
            string matchingFunctionId = null;
            foreach(EventInfo eInfo in events)
            {
                if(eInfo.name == incomingEvent.MessageId)
                {
                    bool match = MatchEventParams(incomingEvent, eInfo.parameters);

                    if((match == true) && (matchingFunctionId == null))
                    {
                        mostNumMatchingParams = eInfo.parameters.Count;
                        matchingFunctionId = eInfo.functionId;
                    }
                    else if((match == true) && (eInfo.parameters.Count > mostNumMatchingParams))
                    {
                        mostNumMatchingParams = eInfo.parameters.Count;
                        matchingFunctionId = eInfo.functionId;
                    }
                }
            }

            return matchingFunctionId;
        }

        public void Clear()
        {
            events.Clear();
        }

        public EventCollection Clone()
        {
            EventCollection _new = new EventCollection();
            
            EventInfo eInfo = null;
            for(int i=0; i<events.Count; i++)
            {
                eInfo = events[i] as EventInfo;
                _new.events.Add(eInfo.Clone());
            }

            _new.gotTrigger = this.gotTrigger;

            return _new;
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return events.GetEnumerator();
		}

		#endregion
	}
}