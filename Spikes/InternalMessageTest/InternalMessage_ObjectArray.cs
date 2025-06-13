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
        private object[] fields;
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

        // Get all the fields
        // Use this only if there's no other way
        public ArrayList Fields
        {
            get
            {
                ArrayList fields = new ArrayList();

                for(int i=0; i<fieldOffset; i++)
                {
                    object[] vals = fields[i] as object[];
                    if(vals != null && vals.Length > 1)
                    {
                        string name = vals[0] as string;
                        if(name != null && name != String.Empty)
                        {
                            for(int x=1; x<vals.Length; x++)
                            {
                                fields.Add(new Field(name, vals[x]));
                            }
                        }
                    }
                }
                return fields;
            }
        }

        public InternalMessage(int initNumFields)
        {
            this.fields = new object[initNumFields];
        }

        public InternalMessage()
            : this(DefNumFields) { }

		public void AddField(string name, object Value)
        {
            if(name == null) { return; }
            if(Value == null) { return; }

            AddFields(name, new object[] { Value });
        }

        public void AddFields(string name, params object[] Values)
        {
            if(name == null) { return; }
            if(Values == null) { return; }

            int offset = 0;

            int foundIndex = -1;
            object[] vals = FindFieldArray(name, out foundIndex);
            if(vals == null)
            {
                vals = new object[Values.Length + 1];
                vals[0] = name;
                offset = 1;
            }
            else
            {
                offset = vals.Length;
                vals = GrowObjArray(vals, Values.Length);
            }

            for(int i=0; i<Values.Length; i++)
            {
                vals[i+offset] = Values[i];
            }

            if(foundIndex >= 0)
            {
                fields[foundIndex] = vals;
            }
            else
            {
                fields[fieldOffset] = vals;

                fieldOffset++;
                if(fieldOffset == fields.Length)
                {
                    fields = GrowObjArray(fields, DefNumFields);
                }
            }
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

            int foundIndex = -1;
            object[] vals = FindFieldArray(name, out foundIndex);
            if(vals == null)
                return null;

            object[] retVals = new object[vals.Length-1];

            for(int i=0; i<retVals.Length; i++)
            {
                retVals[i] = vals[i+1];
            }

            return retVals;
		}

        public bool Contains(string name)
        {
            if(name == null)
                return false;

            for(int i=0; i<fieldOffset; i++)
            {
                object[] vals = fields[i] as object[];
                if(vals != null && vals.Length > 0)
                {
                    string fName = vals[0] as string;
                    if(fName == name)
                        return true;
                }
            }
            return false;
        }

        public object RemoveField(string name)
        {
            if(name == null)
                return null;

            object Value = null;

            // This one's tricky with a native array
            // Just set the field name and value(s) to null
            // but leave the object[] in there
            for(int i=0; i<fieldOffset; i++)
            {
                object[] vals = fields[i] as object[];
                if(vals != null && vals.Length > 0)
                {
                    string fName = vals[0] as string;
                    if(fName == name)
                    {
                        if(vals.Length == 2)
                            Value = vals[1];

                        for(int x=0; x<vals.Length; x++)
                        {
                            vals[x] = null;
                        }

                        return true;
                    }
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

        private object[] FindFieldArray(string name, out int fieldIndex)
        {
            fieldIndex = -1;

            for(int i=0; i<fieldOffset; i++)
            {
                object[] vals = fields[i] as object[];
                if(vals != null && vals.Length > 1)
                {
                    string fName = vals[0] as string;
                    if(fName == name)
                    {
                        fieldIndex = i;
                        return vals;
                    }
                }
            }
            return null;
        }

        private object[] GrowObjArray(object[] array, int howMuch)
        {
            object[] newArray = new object[array.Length + howMuch];

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
