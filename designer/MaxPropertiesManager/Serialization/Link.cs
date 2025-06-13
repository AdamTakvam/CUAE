using System.Xml.Serialization;
 
namespace Metreos.Max.Framework.Satellite.Property 
{
    
    
  /// <remarks/>
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Link.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)]//, Namespace="http://tempuri.org/Link.xsd", IsNullable=false)]
  public class MaxLinkType 
  {  
    public MaxLinkType(){}

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrText)]
    public string text;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrStyle)]
    public string style;

    [System.Xml.Serialization.XmlArrayItemAttribute(Defaults.xmlEltLogging, typeof(LogType), IsNullable=true)]
    public LogType logging;
  }
}
