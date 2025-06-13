using System.Xml.Serialization;

namespace Metreos.Max.Framework.Satellite.Property 
{ 
  /// <remarks/>
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)]
  public class MaxProjectType
  {        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrName)]
    public string name;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrDescription)]
    public string description;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrCompany)]
    public string company;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrAuthor)]
    public string author;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrCopyright)]
    public string copyright;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrTrademark)]
    public string trademark;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrVersion)]
    public string version;

    public @using[] usings;
  }

  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltUsing)]
  public class @using
  {
     [System.Xml.Serialization.XmlTextAttribute()]
     public string Value;
  }
}
