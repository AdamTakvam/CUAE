using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace TestApp 
{
    [Serializable]
    public class InternalMessage 
    {
        private Hashtable fields;

        private string messageId;
        private string sourceType;
        private string sourceName;
        
        public MessageQueueWriter sourceQueueWriter;


        public Hashtable Fields
        {
            get { return fields; }
        }


        public InternalMessage()
        {
            fields = new Hashtable();
        }
        

        public string MessageId
        {
            get { return (messageId == null) ? "" : messageId; }
            set { messageId = value; }
        }
        

        public string Source
        {
            get { return (sourceName == null) ? "" : sourceName; }
            set { sourceName = value; }
        }


        public string SourceType
        {
            get { return (sourceType == null) ? "" : sourceType; }
            set { sourceType = value; }
        }


        public bool IsFieldPresent(string name)
        {
            return fields.ContainsKey(name);
        }


        public string GetNamespace()
        {
            int lastNamespaceDelim = messageId.LastIndexOf(".");

            return (lastNamespaceDelim > 0) ? messageId.Substring(0, lastNamespaceDelim) : "";
        }
    }
}
