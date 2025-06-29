using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.Types.ClickToTalk.Schemas;

namespace Metreos.Types.ClickToTalk
{
	/// <summary>Holds the info about the received InitiateCall request</summary>
	public class MakeCallInfo : IVariable
	{
        private object Value;

        public string Username { get { return Value != null ? ((initCallType)Value).username : ""; } }
        public string Password { get {return Value != null ? ((initCallType)Value).password : ""; } }
        public bool Record { get { return Value != null ? ((initCallType)Value).record : false; } }
        public string EmailAddress { get { return Value != null ? ((initCallType)Value).email : ""; } }
        public calleeType[] Callees { get { return Value != null ? ((initCallType)Value).callee : null; } }

        public MakeCallInfo()
        {}

        public void Reset()
        {
            Value = null;
        }

        [TypeInput("string", "The body of a ClickToTalk HTTP request")]
        public bool Parse(string newValue)
        {
            if(newValue == null) { return false; }

            // Deserialize the InitiateCall message body
            StringReader callInfoStream = new StringReader(newValue);
            XmlSerializer serializer = new XmlSerializer(typeof(initCallType));

            try
            {
                Value = (initCallType) serializer.Deserialize(callInfoStream);
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                callInfoStream.Close();
            }
            
            return true;
        }

        public override string ToString()
        {
            if(Value == null) { return ""; }

            XmlSerializer serializer = new XmlSerializer(typeof(initCallType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, Value);
            writer.Close();

            return sb.ToString();
        }
	}

    public abstract class Hack
    {
        public static string GetCallId(Hashtable connectionHash)
        {
            if(connectionHash == null) { return null; }

            IDictionaryEnumerator de = connectionHash.GetEnumerator();
            de.MoveNext();
            return de.Key as string;
        }
    }
}
