using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;

using Metreos.Common.Mec;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Types.Mec
{
    /// <summary>Holds the info in web response</summary>
    [Serializable]
    public class WebMessageResponse : IVariable
    {
        private object Value;

    
        public WebMessageResponse() 
        {
            Reset();
        }
                
        public void Reset()
        {
            Value = new conferenceResponseType();
        }

        public bool Parse(string newValue)
        {
            System.IO.TextReader reader = new System.IO.StringReader(newValue);
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));

            try
            {
                Value = (conferenceResponseType) serializer.Deserialize(reader);
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public bool Parse(object obj)
        {
            try
            {
                Value = (conferenceResponseType) obj;
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceResponseType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, Value);
            writer.Close();

            serializer = null;
            writer = null;

            return sb.ToString();
        }
    }
}
