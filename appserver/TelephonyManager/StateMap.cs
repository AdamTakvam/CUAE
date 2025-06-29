using System;
using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;

using Metreos.LoggingFramework;

namespace Metreos.AppServer.TelephonyManager
{
	internal sealed class StateMap
	{
        private class Tags
        {
            public const string TRIGGER             = "trigger:";
            public const string ACTION              = "a:";
            public const string RESPONSE_NEXT_STATE = "rns";
            public const string EVENT_NEXT_STATE    = "ens";
            public const string ACTION_NEXT_STATE   = "ans";
            public const string DATA_NEXT_STATE     = "dns";
            public const string DEFAULT_NEXT_STATE  = "def";
            public const string TIMEOUT             = "timeout";
        }

        public StateDescription this[uint stateId] { get { return map[stateId] as StateDescription; } }

        private Hashtable map;   // State ID -> StateDescription

        private uint firstStateId = uint.MaxValue;
        public uint FirstStateId { get { return firstStateId; } }

		public StateMap()
		{
            map = new Hashtable();
		}

        public bool Contains(uint stateId)
        {
            return map.ContainsKey(stateId);
        }

        public bool LoadScript(string script, string scriptName, LogWriter log, out string eventName)
        {
            eventName = null;
            if((script == null) || (scriptName == null)) { return false; }
            script = script.Trim();

            if(script.StartsWith(Tags.TRIGGER))
            {
                int index = script.IndexOf("\r\n\r\n");
                eventName = script.Substring(0, index);
                eventName = eventName.Replace(Tags.TRIGGER, "");
                script = script.Substring(index+4, script.Length-(index+4)); // Cut out the event name
            }

            string[] states = Regex.Split(script, "^.$", RegexOptions.Multiline);

            foreach(string state in states)
            {
                string trimmedState = state.Trim();
                string[] stateLines = trimmedState.Split('\n');
                if(stateLines.Length < 2)
                {
                    log.Write(TraceLevel.Error, "Incomplete state description in {0}: {1}", scriptName, state);
                    map.Clear();
                    return false;
                }

                // Get state ID
                uint stateId = Convert.ToUInt32(stateLines[0]);
                if(stateId == 0)
                {
                    log.Write(TraceLevel.Error, "Invalid state ID in {0}: {1}", scriptName, state);
                    map.Clear();
                    return false;
                }

                if(stateId < firstStateId)
                {
                    this.firstStateId = stateId;  // Load firstStateId with the lowest one
                }

                StateDescription desc = ParseStateDescription(stateLines, stateId, scriptName, log);
                if(desc == null) { return false; }

                this.map[stateId] = desc;
            }
 
            return ValidateMap();
        }

        private StateDescription ParseStateDescription(string[] stateLines, uint stateId, 
            string scriptName, LogWriter log)
        {
            StateDescription desc = new StateDescription();

            for(int i=1; i<stateLines.Length; i++)
            {
                string line = stateLines[i].Trim();
                    
                int commentIndex = line.IndexOf("//");
                if(commentIndex != -1)
                {
                    line = line.Substring(0, commentIndex).TrimEnd();
                }

                string[] lineBits = Regex.Split(line, "->");

                if(lineBits.Length == 1)
                {
                    // Must be action
                    if(lineBits[0].StartsWith(Tags.ACTION))
                    {
                        string actionName = lineBits[0].Replace(Tags.ACTION, "");
                        try
                        {
                            desc.action = (Actions) Enum.Parse(typeof(Actions), actionName, true);
                        }
                        catch
                        {
                            log.Write(TraceLevel.Error, "Invalid action found on line {0} of state {1}:{2}",
                                i.ToString(), scriptName, stateId.ToString());
                            return null;
                        }
                    }
                    else
                    {
                        log.Write(TraceLevel.Error, "Syntax error on line {0} of {1}:{2}", 
                            i.ToString(), scriptName, stateId.ToString());
                        return null;
                    }
                }
                else if(lineBits.Length > 2)
                {
                    log.Write(TraceLevel.Error, "Syntax error (too many arrows) on line {0} of {1}:{2}", 
                        i.ToString(), scriptName, stateId.ToString());
                    return null;
                }
                else
                {
                    // The part after the arrow can be a state ID or another script name (in brackets)
                    string nextStateId = lineBits[1];
                    if(nextStateId == String.Empty)
                    {
                        log.Write(TraceLevel.Error, "Syntax error (invalid ID after arrow) on line {0} of {1}:{2}", 
                            i.ToString(), scriptName, stateId.ToString());
                        return null;
                    }

                    string[] arrayBits = lineBits[0].Split(':');
                    string tag = arrayBits[0];

                    if(arrayBits.Length == 1)
                    {
                        switch(tag)
                        {
                            case Tags.DEFAULT_NEXT_STATE:
                                desc.defaultNextState = nextStateId;
                                break;
                            default:
                                log.Write(TraceLevel.Error, "Syntax error on line {0} of {1}:{2}", 
                                    i.ToString(), scriptName, stateId.ToString());
                                return null;
                        }
                    }
                    else if(arrayBits.Length > 2)
                    {
                        log.Write(TraceLevel.Error, "Syntax error (too many colons) on line {0} of {1}:{2}", 
                            i.ToString(), scriptName, stateId.ToString());
                        return null;
                    }
                    else if(arrayBits[1] == String.Empty)
                    {
                        log.Write(TraceLevel.Error, "Syntax error on line {0} of {1}:{2}", 
                            i.ToString(), scriptName, stateId.ToString());
                        return null;
                    }
                    else
                    {
                        string _value = arrayBits[1].ToLower();

                        switch(tag)
                        {
                            case Tags.ACTION_NEXT_STATE:
                                if(desc.actionNextStates == null) { desc.actionNextStates = new Hashtable(); }
                                desc.actionNextStates[_value] = nextStateId;
                                break;
                            case Tags.EVENT_NEXT_STATE:
                                if(desc.eventNextStates == null) { desc.eventNextStates = new Hashtable(); }
                                desc.eventNextStates[_value] = nextStateId;
                                break;
                            case Tags.RESPONSE_NEXT_STATE:
                                if(desc.responseNextStates == null) { desc.responseNextStates = new Hashtable(); }
                                desc.responseNextStates[_value] = nextStateId;
                                break;
                            case Tags.TIMEOUT:
                                if(desc.SetTimeout(Convert.ToUInt32(_value), nextStateId) == false)
                                {
                                    log.Write(TraceLevel.Error, "Syntax error on line {0} of {1}:{2}", 
                                        i.ToString(), scriptName, stateId.ToString());
                                    return null;
                                }
                                break;
                            case Tags.DATA_NEXT_STATE:
                                if(desc.dataNextStates == null) { desc.dataNextStates = new Hashtable(); }
                                DataCriteria crit = CreateDataCriteria(_value, scriptName, i, stateId, log);
                                if(crit == null) { return null; }
                                desc.dataNextStates.Add(crit, nextStateId);
                                break;
                            default:
                                log.Write(TraceLevel.Error, "Syntax error on line {0} of {1}:{2}", 
                                    i.ToString(), scriptName, stateId.ToString());
                                return null;
                        }
                    }
                }    
            }

            return desc;
        }

        private DataCriteria CreateDataCriteria(string _value, string scriptName, 
            int lineNum, uint stateId, LogWriter log)
        {
            DataCriteria crit = new DataCriteria();

            // Figure out which operator it is
            string[] valueBits = Regex.Split(_value, "!=");
            if(valueBits.Length == 1)
            {
                valueBits = _value.Split('=');
                if(valueBits.Length != 2)
                {
                    log.Write(TraceLevel.Error, "Unrecognized comparator in data criteria on line {0} of {1}:{2}",
                        lineNum.ToString(), scriptName, stateId.ToString());
                    return null;
                }

                crit.comparator = Comparators.Equal;
            }
            else
            {
                crit.comparator = Comparators.NotEqual;
            }
                                    
            if(valueBits.Length != 2)
            {
                log.Write(TraceLevel.Error, "Syntax error on line {0} of {1}:{2}", 
                    lineNum.ToString(), scriptName, stateId.ToString());
                return null;
            }

            // Parse the field name                        
            try 
            {
                crit.fieldName = 
                    (DataFields)Enum.Parse(typeof(DataFields), valueBits[0], true);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Invalid data field specified on line {0} of {1}:{2}",
                    lineNum.ToString(), scriptName, stateId.ToString());
                return null;
            }

            // Parse the value into the actual type
            object dataVal = null;
            if(valueBits[1].ToLower() != "null")
            {
                try
                {
                    switch(crit.fieldName)
                    {
                        // Integers
                        case DataFields.conferenceId:
                            dataVal = Convert.ToUInt32(valueBits[1]);
                            break;
                        case DataFields.peerCallId:
                            dataVal = Convert.ToUInt32(valueBits[1]);
                            break;
						// Booleans
						case DataFields.conference:
						case DataFields.negCaps:
                        case DataFields.earlyMedia:
                            dataVal = bool.Parse(valueBits[1]);
                            break;
                        // WaitMedia enum
                        case DataFields.waitForMedia:
                            dataVal = (WaitMedia)Enum.Parse(typeof(WaitMedia), valueBits[1], true);
                            break;
                        // Other value types
                        default:
                            throw new ArgumentException("Reference types can only be compared to null.");
                    }
                }
                catch
                {
                   log.Write(TraceLevel.Error, "Invalid data value specified on line {0} of {1}:{2}",
                      lineNum.ToString(), scriptName, stateId.ToString());
                   return null;
                }
            }
            
            crit.Value = dataVal;
            return crit;
        }

        private bool ValidateMap()
        {
            // TODO: Verify all next state IDs exist, all states have next states, etc.
            return true;
        }
	}
}
