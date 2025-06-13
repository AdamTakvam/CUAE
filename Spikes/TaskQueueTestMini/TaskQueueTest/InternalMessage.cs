using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Metreos.Messaging
{
    [Serializable]
    public class InternalMessage
    {
        public ArrayList fieldValues = new ArrayList();
        
        public Hashtable fieldIndex = CollectionsUtil.CreateCaseInsensitiveHashtable();

        public object[] objArray = new object[100];

        public long Time;
        public int ThreadId;
        public string sourceName;
        public string messageId;

        public string MessageId
        {
            get { return (messageId == null) ? "" : messageId; }
            set { messageId = value; }
        }

        public InternalMessage()
        { }
    }
}
