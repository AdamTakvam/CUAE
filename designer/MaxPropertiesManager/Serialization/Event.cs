using Metreos.ApplicationFramework.ScriptXml;
using System.Xml.Serialization;
using PropertyGrid.Core; 
namespace Metreos.Max.Framework.Satellite.Property 
{
    
    
  /// <remarks/>
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Event.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)] //, Namespace="http://tempuri.org/Event.xsd", IsNullable=false)]
  public class MaxEventType 
  {        
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Defaults.xmlEltEventParameter)]
    public eventParamType[] @params;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrType)]
    public EventType eventType;
  }
}
