using System;
using System.Xml.Serialization;

namespace Metreos.RecordAgent
{
    /// <summary>
    /// Summary description for Record.
    /// </summary>
    [Serializable()]
    public class Record
    {
        public Record()
        {
        }
        [XmlAttributeAttribute("Key")]
        public string key;

        [XmlAttributeAttribute("Topic")]
        public string topic;

        [XmlAttributeAttribute("Name")]
        public string name;

        [XmlAttributeAttribute("Number")]
        public string number;

        [XmlAttributeAttribute("CallDateTime")]
        public string callDateTime;

        [XmlAttributeAttribute("Flag")]
        public bool flag;

        [XmlArrayAttribute("AudioFiles")]
        public AtomicAudioFile[] AtomicAudioFiles;

        public string GetDataString()
        {
            return topic + " " + callDateTime + " " + name + " " + number;
        }
    }

    public class AtomicAudioFile
    {
        public AtomicAudioFile()
        {
        }

        public string fileName;
        public int duration;            // in secs
    }
}
