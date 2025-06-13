using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace TaskQueueTest
{
    /// <summary>
    /// Defines a message container for messaging between individual Samoa
    /// components. InternalMessage is fully serializable.
    /// </summary>
    [Serializable]
    public class InternalMessage
    {
        public int ThreadId;

        protected const int DefNumFields = 10;
        protected const double GrowthFactor = 1.5;

        // Hash of field name -> int. The integer is the index at which the field's values are 
        // stored in fieldValues. This allows fast lookup for multiple fields with the same name.
        [NonSerialized]
        protected  Hashtable fieldIndices;

        // The field valsArray
        protected object[] fieldValues;
        protected uint lastIndex = 0;

        protected string routingGuid;
        protected string sourceName;
        protected string destName;
        protected string messageId;

        // Indexer
        public object this[string fieldName]
        {
            get { return GetField(fieldName); }
        }

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

        /// <summary>Get and set the message ID. If the message ID is null, then 
        ///          <code>String.Empty</code> is returned.</summary>
        public string MessageId
        {
            get { return (messageId == null) ? String.Empty : messageId; }
            set { messageId = value; }
        }

        /// <summary>Check whether this message has a non-null source and message ID.</summary>
        public virtual bool IsComplete
        {
            get
            {
                return sourceName != null && messageId != null;
            }
        }

        /// <summary>Retrieve the number of fields in this message. If multiple values exist for a single
        ///          field name then they will all be counted.</summary>
        public int Count
        {
            get
            {
                object[] objArray;
                int count = 0;
                for(int i = 0; i < fieldValues.Length; i++)
                {
                    if(fieldValues[i] != null)
                    {
                        objArray = (object[]) fieldValues[i];
                        count += objArray.Length;
                    }
                }
                return count;
            }
        }

        /// <summary>Retrieve an <code>ArrayList</code> of <code>Field</code> objects representing all
        ///          name/value pairs stored in this <code>InternalMessage</code>. The ordering of the
        ///          fields in the <code>ArrayList</code> is alphabetical based on field name.</summary>
        /// <remarks>This should only be used when necessary. It is inherently heavy weight. If you just
        ///          need to retrieve the number of fields contained in the message, use the provided
        ///          <code>Count</code> property on the message itself.</remarks>
        public ArrayList Fields
        {
            get
            {
                ArrayList fields = new ArrayList();

                foreach(DictionaryEntry de in fieldIndices)
                {
                    uint index = (uint) de.Value;

                    object[] values = (object[]) fieldValues[index];
                    if(values != null)
                    {
                        for(int i = 0; i < values.Length; i++)
                        {
                            fields.Add(new Field((string) de.Key, values[i]));
                        }
                    }
                }

                fields.Sort();
                return fields;
            }
        }

        /// <summary>Default constructor. The <code>InternalMessage</code> will be initialized with a
        ///          default size.</summary>
        public InternalMessage() : this(DefNumFields) 
        { }
        
        /// <summary>Constructor. Create an <code>InternalMessage</code> with the specified size.</summary>
        /// <param name="initNumFields">The number of initial field entries to create.</param>
        public InternalMessage(int initNumFields)
        {
            fieldValues = new object[initNumFields];
            fieldIndices = CollectionsUtil.CreateCaseInsensitiveHashtable();
        }

        /// <summary>Add a new field with a single value to this message.</summary>
        /// <param name="name">The name, or key, to assign to this field.</param>
        /// <param name="val">The value to assign to this field.</param>
		public void AddField(string name, object val)
        {
            if((name == null) || (val == null))
                return;

            AddFields(name, new object[] { val });
        }
        
        /// <summary>Add a new field with, potentially, multiple valsArray to this message.</summary>
        /// <param name="name">The name, or key, to assign to this field.</param>
        /// <param name="values">One or more values to assign to this field.</param>
        public void AddFields(string name, params object[] values)
        {
            if((name == null) || (values == null))
                return;

            object[] valsArray;
            uint index;

            if(fieldIndices.ContainsKey(name))
            {
                // Multiple values for the same key
                index = (uint) fieldIndices[name];
                valsArray = (object[]) fieldValues[index];

                // Grow array, put in old values first, and then the new ones
                object[] newValues = new object[valsArray.Length + values.Length];
                valsArray.CopyTo(newValues, 0);
                values.CopyTo(newValues, valsArray.Length);
                fieldValues[index] = newValues;
            }
            else
            {
                valsArray = new object[values.Length];
                values.CopyTo(valsArray, 0);
                index = lastIndex++;
                fieldIndices[name] = index;

                if(index >= fieldValues.Length)
                {
                    int newSize = (int) Math.Round(fieldValues.Length * GrowthFactor, 0);

                    // Growth the fieldValues array to accomodate more stuff.
                    object[] newFieldValues = new object[newSize];
                    fieldValues.CopyTo(newFieldValues, 0);
                    fieldValues = newFieldValues;
                }

                fieldValues[index] = valsArray;
            }
        }
        
        /// <summary>Retrieve a field with one, and only one, value associated with it.</summary>
        /// <param name="name">The name of the field to retrieve.</param>
        /// <returns>An <code>object</code> with the value of the field. If the field is not
        ///          present or if there are multiple valsArray associated with the field then 
        ///          <code>null</code> is returned.</returns>
        public object GetField(string name)
        {
            object[] values = GetFields(name);
            return values != null && values.Length == 1 ? values[0] : null;
        }

        /// <summary>Retrieve all valsArray for a given field.</summary>
        /// <param name="name">The name of the field to retrieve.</param>
        /// <returns>An array of type <code>object</code> with the valsArray for the field. If the field is
        ///          not present in the message then <code>null</code> is returned.</returns>
        public object[] GetFields(string name)
        {
            if(Contains(name) == false)
                return null;

            uint index = (uint) fieldIndices[name];
            object[] values = (object[]) fieldValues[index];

            return values;
		}

        /// <summary>Check whether an <code>InternalMessage</code> contains a field with a specific name.</summary>
        /// <param name="name">The name of the field to look for.</param>
        /// <returns>True if a field with <code>name</code> is present, false if <code>name</code> 
        ///          is null or is not present.</returns>
        public bool Contains(string name)
        {
            if(name == null)
                return false;

            return fieldIndices.ContainsKey(name);
        }

        /// <summary>Remove a field and all of its values from this <code>InternalMessage</code>.</summary>
        /// <param name="name">The name, or key, of the field to remove.</param>
        /// <returns>The value of the field that was removed. If multiple values are associated with the
        ///          same key then only the first value is returned. <code>null</code> is returned if the 
        ///          field is not present or if <code>name</code> is <code>null</code>.</returns>
        public object RemoveField(string name)
        {
            if(Contains(name) == false)
                return null;

            uint index = (uint) fieldIndices[name];

            object[] vals = (object[]) fieldValues[index];
            fieldValues[index] = null;
            fieldIndices.Remove(name);

            return vals[0];
        }

        /// <summary>Create a string representation of this message.</summary>
        /// <param name="msgType">The type of the message.</param>
        /// <param name="subClassMembers">If the message contains any sub class members we are interested in.</param>
        /// <returns>A <code>string</code> with this messages data.</returns>
        public string ToString(string msgType, StringDictionary subClassMembers)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("InternalMessage:");
            sb.Append("  Type: ");
            sb.AppendLine(msgType);
            sb.Append("  Message ID: ");
            sb.AppendLine(messageId == null ? "unspecified" : messageId);
            sb.Append("  Routing GUID: ");
            sb.AppendLine(routingGuid == null ? "unspecified" : routingGuid);
            sb.Append("  Source: ");
            sb.AppendLine(sourceName == null ? "unspecified" : sourceName);
            sb.Append("  Destination: ");
            sb.AppendLine(destName == null ? "unspecified" : destName);
            
            if(subClassMembers != null)
            {
                foreach(DictionaryEntry de in subClassMembers)
                {
                    sb.AppendFormat("  {0}: ", de.Key);
                    sb.AppendLine(de.Value as string);
                }
            }

            sb.AppendLine("Fields:");
            
            ArrayList fields = this.Fields;
            foreach(Field f in fields)
            {
                sb.AppendFormat("  {0}: ", f.Name);
                sb.AppendLine(f.Value.ToString());
            }

            return sb.ToString();
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
    public sealed class Field : IComparable
    {
        public string Name;
        public object Value;

        public Field(string name, object Value)
        {
            this.Name  = name;
            this.Value = Value;
        }  

        #region IComparable

        public int CompareTo(object obj)
        {
            if(obj is Field)
            {
                Field field = (Field) obj;
                return Name.CompareTo(field.Name);
            }
            throw new ArgumentException("obj is not a Field");
        }

        #endregion
    }
}
