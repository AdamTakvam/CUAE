using System;
using System.Xml.Serialization;

namespace Metreos.Common.Reserve
{
    [Serializable]
    [System.Xml.Serialization.XmlRootAttribute("DisplayRequest", Namespace="", IsNullable=false)]
    public class DisplayRequestType 
    {
        [XmlElement("p")]
        public int pollMinutes;

        [XmlElement("i")]
        public DisplayItem[] items;    
    }

    public class DisplayItem
    {
        [XmlText()]
        public string Value;

        [XmlAttribute("u")]
        public string User;
    
        [XmlAttribute("f")]
        public string First;
   
        [XmlAttribute("l")]
        public string Last;

        [XmlAttribute("r")]
        public string RecordId;
    }
}