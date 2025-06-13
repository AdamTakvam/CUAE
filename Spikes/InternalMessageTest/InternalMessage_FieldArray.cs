using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Metreos.Messaging
{
    [Serializable]
    public class InternalMessage : IDisposable
    {
        private const int DefNumFields = 15;

        // The field values
        private Field[] fields;
        private int fieldOffset = 0;

        protected string routingGuid;
        protected string sourceName;
        protected string destName;
        protected string messageId;

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

        public ArrayList Fields { get { return new ArrayList(fields); } }

        public InternalMessage(int initNumFields)
        {
            this.fields = new Field[initNumFields];
        }

        public InternalMessage()
            : this(DefNumFields) { }

		public void AddField(string name, object Value)
        {
            if((name == null) ||
               (Value == null))
                return;

            AddFields(name, new object[] { Value });
        }

        public void AddFields(string name, params object[] Values)
        {
            if((name == null) ||
               (Values == null))
                return;

            foreach(object val in Values)
            {
                fields[fieldOffset] = new Field(name, val);

                fieldOffset++;
                if(fieldOffset == fields.Length)
                {
                    fields = GrowFieldArray(fields, Values.Length);
                }
            }
        }
        
        public object GetField(string name)
        {
            for(int i=0; i<fieldOffset; i++)
            {
                Field f = fields[i];
                if(f != null && f.Name == name)
                    return f.Value;
            }
            return null;
        }

        public object[] GetFields(string name)
        {
            if(name == null)
                return null;

            ArrayList values = new ArrayList();

            for(int i=0; i<fieldOffset; i++)
            {
                Field f = fields[i];
                if(f != null && f.Name == name)
                    values.Add(f.Value);
            }

            if(values.Count > 0)
            {
                object[] valArray = new object[values.Count];
                values.CopyTo(valArray);
                return valArray;
            }

            return null;
		}

        public bool Contains(string name)
        {
            if(name == null)
                return false;

            for(int i=0; i<fieldOffset; i++)
            {
                Field f = fields[i];
                if(f != null && f.Name == name)
                    return true;
            }
            return false;
        }

        public object RemoveField(string name)
        {
            if(name == null)
                return null;

            object Value = null;

            // This one's tricky with a native array
            // Just set the field name and value to null
            // but leave the Field object in there
            for(int i=0; i<fieldOffset; i++)
            {
                Field f = fields[i];
                if(f != null && f.Name == name)
                {
                    Value = f.Value;

                    f.Name = null;
                    f.Value = null;
                }
            }

            return Value;
        }

        public void Dispose()
        {
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
            
            for(int i=0; i<fieldOffset; i++)
            {
                Field field = fields[i];
                if(field != null)
                {
                    sb.Append("\n  ");
                    sb.Append(field.Name);
                    sb.Append(": ");
                    sb.Append(field.Value == null ? "<null>" : field.Value.ToString());
                }
            }

            return sb.ToString();
        }

        private Field[] GrowFieldArray(Field[] array, int amount)
        {
            Field[] newArray = new Field[array.Length + amount];

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
}
