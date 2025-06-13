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
    /// <summary>Holds the info in web request</summary>
    [Serializable]
    public class WebMessageRequest : IVariable
    {
        private object Value;
    
        // Proprietary Properties
        public int NumLocations
        { 
            get 
            { 
                return ((conferenceRequestType)Value).location.Length; 
            } 
        }

        public string StartTime
        {
            get
            {
                conferenceRequestType cr = (conferenceRequestType) Value;
                if(cr.startTime == null)
                {
                    return System.DateTime.Now.ToString();
                }
                return cr.startTime;
            }
        }

        // Proprietary methods
        public string GetLocationId(int index)
        {
            conferenceRequestType cr = (conferenceRequestType) Value;
            if(index >= cr.location.Length)
            {
                return "";
            }
            return cr.location[index].Value; 
        }

        public string GetContactAddress(int index)
        {
            conferenceRequestType cr = (conferenceRequestType) Value;

            if(cr.location == null)
            {
                return "";
            }

            if(index >= cr.location.Length)
            {
                return "";
            }
            return cr.location[index].address;
        }

        public bool IsMuted(int index)
        {
            conferenceRequestType cr = (conferenceRequestType) Value;
            if(index >= cr.location.Length)
            {
                return true;
            }
            if(!cr.location[index].mutedSpecified)
            {
                return false;
            }
            return cr.location[index].muted;
        }

        public bool IsMuted(string address)
        {
            locationType locationData = GetLocationData(address);
            if(locationData == null)
            {
                return true;
            }
            if(!locationData.mutedSpecified)
            {
                return false;
            }
            return locationData.muted;
        }
        
        public string GetDescription(int index)
        {
            conferenceRequestType cr = (conferenceRequestType) Value;
            if(index >= cr.location.Length)
            {
                return "";
            }
            if(cr.location[index].description == null)
            {
                return "";
            }
            return cr.location[index].description;
        }

        public string GetDescription(string address)
        {
            locationType locationData = GetLocationData(address);
            if(locationData == null)
            {
                return "";
            }
            if(locationData.description == null)
            {
                return "";
            }
            return locationData.description;
        }

        public WebMessageRequest() 
        {
            Reset();
        }
                
        public void Reset()
        {
            Value = new conferenceRequestType();
        }

        public bool Parse(string newValue)
        {
            System.IO.TextReader reader = new System.IO.StringReader(newValue);
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));

            try
            {
                Value = (conferenceRequestType) serializer.Deserialize(reader);
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
                Value = (conferenceRequestType) obj;
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(conferenceRequestType));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, Value);
            writer.Close();

            serializer = null;
            writer = null;

            return sb.ToString();
        }

        // Private methods
        private locationType GetLocationData(string address)
        {
            conferenceRequestType cr = (conferenceRequestType) Value;
            for(int i=0; i<cr.location.Length; i++)
            {
                if(cr.location[i].address == address)
                {
                    return cr.location[i];
                }
            }

            return null;
        }
    }
}
