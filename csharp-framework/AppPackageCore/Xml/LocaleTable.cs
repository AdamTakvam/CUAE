// Modified heavily from xsd.exe because I can't get it to interpret CDATA correctly. MSC
using System.Xml;
using System.Xml.Serialization;

namespace Metreos.AppArchiveCore.Xml
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cisco.com/LocaleTable.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("LocaleTable", Namespace = "http://www.cisco.com/LocaleTable.xsd", IsNullable = false)]
    public class LocaleTableType
    {
        [System.Xml.Serialization.XmlElementAttribute("Locales")]
        public Locales Locales;

        [System.Xml.Serialization.XmlElementAttribute("Prompts")]
        public Prompts Prompts;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.cisco.com/LocaleTable.xsd")]
    public class Locales {
        
        [System.Xml.Serialization.XmlElementAttribute("Locale")]
        public Locale[] Locale;
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @default;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool @readonly;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cisco.com/LocaleTable.xsd")]
    public class Locale
    {
        [System.Xml.Serialization.XmlElementAttribute("PromptInfo", IsNullable = true)]
        public PromptInfo[] PromptInfos;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool devmode;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int width;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cisco.com/LocaleTable.xsd")]
    public class PromptInfo
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @ref;

        public XmlCDataSection Value;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cisco.com/LocaleTable.xsd")]
    public class Prompts
    {

        [System.Xml.Serialization.XmlElementAttribute("Prompt")]
        public Prompt[] Prompt;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cisco.com/LocaleTable.xsd")]
    public class Prompt
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int height;
    }
}
