using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

using Metreos.Messaging;

namespace csipctest
{
    /// <summary>
    /// Defines a message container for messaging between individual Samoa
    /// components. MediaServerMessage is fully serializable to/from XML.
    /// </summary>
    [XmlRootAttribute(ElementName = "message", IsNullable = false)]
    public class MediaServerMessage 
    {
        private ArrayList listFields;
        private string messageId;

        public MediaServerMessage()
        {
            listFields = new ArrayList();
        }

        public void AddField(string name, string Value)
        {
            AddField(new Field(name, Value));
        }

        public void AddField(Field field)
        {
            listFields.Add(field);
        }

        public void AddFields(string name, object[] Values)
        {
            foreach(string val in Values)
            {
                this.AddField(name, val);
            }
        }

        public ArrayList GetFields()
        {
            return listFields;
        }

        [XmlElement(ElementName = "field")]
        public Field[] Fields
        {
            get
            {
                Field[] fields = new Field[listFields.Count];
                listFields.CopyTo(fields);
                return fields;
            }

            set
            {
                if(value == null) { return; }

                Field[] fields = (Field[])value;
                listFields.Clear();
                
                foreach(Field field in fields)
                {
                    listFields.Add(field);
                }
            }
        }
        
        [XmlElement(ElementName = "messageId")]
        public string MessageId
        {
            get { return (messageId == null) ? "" : messageId; }
            set { messageId = value; }
        }

        public bool GetFieldByName(string name, out string fieldValue)
        {
            foreach(Field field in listFields)
            {
                if(field.Name == name)
                {
                    fieldValue = field.Value;
                    return true;
                }
            }

            fieldValue = null;

            return false;
        }

        public bool GetFieldsByName(string name, out string[] fieldValues)
        {
            ArrayList valueList = new ArrayList();

            foreach(Field field in listFields)
            {
                if(field.Name == name)
                {
                    valueList.Add(field.Value);
                }
            }

            fieldValues = (string[]) valueList.ToArray(typeof(string));

            if(fieldValues.Length > 0)
            {
                return true;
            }

            fieldValues = null;
            return false;
        }

        public bool IsFieldPresent(string name)
        {
            foreach(Field field in listFields)
            {
                if(field.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveField(string name)
        {
            Field field = null;
            for(int i = 0; i < listFields.Count; i++)
            {
                field = (Field) listFields[i];
                if(field.Name == name)
                {
                    listFields.RemoveAt(i);
                    return;
                }
            }
        }

        public string GetNamespace()
        {
            int lastNamespaceDelim = messageId.LastIndexOf(".");

            return (lastNamespaceDelim > 0) ? messageId.Substring(0, lastNamespaceDelim) : "";
        }

        public override string ToString()
        {
            // Serialize the message into XML.
            //
            // REFACTOR Extract these objects into the class so we aren't 
            // re-creating them all the time. Or would this create too much
            // memory overhead for each InternalMessage?
            //
            XmlSerializer serializer = new XmlSerializer(typeof(MediaServerMessage));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, this);
            writer.Close();

            serializer = null;
            writer = null;

            return sb.ToString();
        }
    }

    
    /// <summary>
    /// A name/value pairing for an InternalMessage. All data contained in a message
    /// that is context specific is encoded using name/value pairs in a Field.
    /// 
    /// <see cref="Metreos.Samoa.Core.InternalMessage"/>
    /// 
    /// </summary>
    ///
    [XmlTypeAttribute(Namespace = "http://metreos.com/InternalMessage.xsd")]
    public class Field 
    {
        private string name;
        private string val;

        public Field()
        {}

        public Field(string fieldName, string fieldValue)
        {
            if((fieldName == null) || (fieldName == ""))
            {
                throw new ArgumentException("Internal message field names cannot be null or empty string");
            }

            if((fieldValue == null) || (fieldValue == ""))
            {
                fieldValue = "NULL";
            }

            Name = fieldName;
            Value = fieldValue;
        }
        
        [XmlAttribute(AttributeName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlTextAttribute()]
        public string Value
        {
            get { return val; }
            set { val = value; }
        }
    }
}
