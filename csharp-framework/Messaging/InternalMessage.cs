using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Interfaces;

namespace Metreos.Messaging
{
    /// <summary>
    /// Defines a message container for messaging between individual Samoa
    /// components. InternalMessage is fully serializable.
    /// </summary>
    [Serializable]
    public abstract class InternalMessage : IDisposable
    {
        // The field values
        private ArrayList fieldValues;

        // Hash of field name -> int[] of field value array indices.
        // This allows fast lookup for multiple fields with the same name
        private Hashtable fieldIndex;

        protected string routingGuid;
        protected string sourceName;
        protected string destName;
        protected string messageId;
        protected IConfig.ComponentType sourceType;
        protected MessageQueueWriter sourceQueue;

        // Indexer
        public object this[string fieldName]
        {
            get { return GetField(fieldName); }
        }

        // Properties
        public string RoutingGuid 
        { 
            get { return routingGuid; } 
            set { routingGuid = value; }
        }  

        public string Destination
        {
            get { return destName; }
            set { destName = value; }
        }

        public string Source
        {
            get { return sourceName; }
            set { sourceName = value; }
        }

        public string MessageId
        {
            get { return (messageId == null) ? "" : messageId; }
            set { messageId = value; }
        }

        public MessageQueueWriter SourceQueue
        {
            get { return sourceQueue; }
            set { sourceQueue = value; }
        }

        public IConfig.ComponentType SourceType
        {
            get { return sourceType; }
            set { sourceType = value; }
        }

        public virtual bool IsComplete
        {
            get
            {
                if((sourceName != null) && (messageId != null))
                {
                    return true;
                }
                return false;
            }
        }

        // Get all the fields
        // Use this only if there's no other way
        public ArrayList Fields
        {
            get
            {
                ArrayList fields = new ArrayList();

                IDictionaryEnumerator de = fieldIndex.GetEnumerator();
                while(de.MoveNext())
                {
                    int[] indices = (int[]) de.Value;
                    for(int i=0; i<indices.Length; i++)
                    {
                        fields.Add(new Field((string)de.Key, fieldValues[indices[i]]));
                    }
                }

                return fields;
            }
        }

        public InternalMessage()
        {
            fieldValues = new ArrayList();
            fieldIndex = CollectionsUtil.CreateCaseInsensitiveHashtable();
        }

		public void AddField(string name, object Value)
        {
            if(name == null) { return; }
            if(Value == null) { return; }

            AddFields(name, new object[] { Value });
        }

        public void AddFields(string name, object[] Values)
        {
            if(name == null) { return; }
            if(Values == null) { return; }

            int offset = 0;
            int[] indices = fieldIndex[name] as int[];
            if(indices == null)
            {
                indices = new int[Values.Length];
            }
            else
            {
                offset = indices.Length;
                indices = GrowIntArray(indices, Values.Length);
            }

            for(int i=0; i<Values.Length; i++)
            {
                int index = fieldValues.Add(Values[i]);
                indices[offset + i] = index;
            }

            fieldIndex[name] = indices;
        }
        
        public object GetField(string name)
        {
            object[] Values = GetFields(name);
            if(Values != null)
            {
                if(Values.Length == 1)
                {
                    return Values[0];
                }
            }
            return null;
        }

        public object[] GetFields(string name)
        {
            if(name == null) { return null; }

            ArrayList values = new ArrayList();

            int[] indices = fieldIndex[name] as int[];

            if(indices != null)
            {
                for(int i=0; i<indices.Length; i++)
                {
                    object valueObj = fieldValues[indices[i]];
                    if(valueObj != null)
                    {
                        values.Add(fieldValues[indices[i]]);
                    }
                }

                if(values.Count > 0)
                {
                    object[] valueArray = new object[values.Count];
                    values.CopyTo(valueArray);
                    return valueArray;
                }
            }

            return null;
		}

        public bool Contains(string name)
        {
            if(name == null) { return false; }
            return fieldIndex[name] != null ? true : false;
        }

        public object RemoveField(string name)
        {
            if(name == null) { return null; }

            object Value = null;

            int[] indices = fieldIndex[name] as int[];
            if(indices != null)
            {
                if(indices.Length == 1)
                {
                    Value = fieldValues[indices[0]];
                    fieldValues[indices[0]] = null;
                }
                else
                {
                    for(int i=0; i<indices.Length; i++)
                    {
                        fieldValues[indices[i]] = null;
                    }
                }

                fieldIndex.Remove(name);
            }

            return Value;
        }

        public void Dispose()
        {
            if(sourceQueue != null)
            {
				try { sourceQueue.Dispose(); }
				catch {}

                sourceQueue = null;
            }
        }

        protected string ToString(string msgType, StringDictionary subClassMembers)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("InternalMessage:\n  Type: ");
            sb.Append(msgType);
            sb.Append("\n  Message ID: ");
            sb.Append(messageId == null ? "unspecified" : messageId);
            sb.Append("\n  Routing GUID: ");
            sb.Append(routingGuid == null ? "unspecified" : routingGuid);
            sb.Append("\n  Source: ");
            sb.Append(sourceName == null ? "unspecified" : sourceName);
            sb.Append("\n  SourceType: ");
            sb.Append(sourceType.ToString());
            sb.Append("\n  SourceQueue: ");
            sb.Append(sourceQueue == null ? "unspecified" : "specified");
            sb.Append("\n  Destination: ");
            sb.Append(destName == null ? "unspecified" : destName);
            
            if(subClassMembers != null)
            {
                foreach(DictionaryEntry de in subClassMembers)
                {
                    sb.Append("\n  ");
                    sb.Append(de.Key);
                    sb.Append(": ");
                    sb.Append(de.Value);
                }
            }

            sb.Append("\nFields:");
            
            ArrayList fields = this.Fields;
            for(int i=0; i<fields.Count; i++)
            {
                Field field = fields[i] as Field;
                sb.Append("\n  ");
                sb.Append(field.Name);
                sb.Append(": ");
                sb.Append(field.Value.ToString());
            }

            return sb.ToString();
        }

        private int[] GrowIntArray(int[] array, int howMuch)
        {
            int[] newArray = new int[array.Length + howMuch];

            for(int i=0; i<array.Length; i++)
            {
                newArray[i] = array[i];
            }

            return newArray;
        }

        #region Field Value Conversions
        private String GetStringValue(object Value)
        {
            if(Value == null) { return null; }

            string val = Value as String;
            if(val != null) 
            {
                return val;
            }

            return Value.ToString();
        }

        private bool GetInt32Value(object Value, out Int32 result)
        {
            result = 0;
            if(Value == null) { return false; }

            try
            {
                result = Int32.Parse(Value.ToString());
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        private bool GetUInt32Value(object Value, out UInt32 result)
        {
            result = 0;
            if(Value == null) { return false; }

            try
            {
                result = UInt32.Parse(Value.ToString());
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
        #endregion
    }

    /// <summary>
    /// This is used to return a list of fields ONLY
    /// </summary>
    [Serializable]
    public sealed class Field
    {
        public Field(string name, object Value)
        {
            this.Name = name;
            this.Value = Value;
        }

        public string Name;
        public object Value;
    }
}
