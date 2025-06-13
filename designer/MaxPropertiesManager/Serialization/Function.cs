using System.Xml.Serialization;
 
namespace Metreos.Max.Framework.Satellite.Property 
{ 
  /// <remarks/>
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Function.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)] //, Namespace="http://tempuri.org/Function.xsd", IsNullable=false)]
  public class MaxFunctionType 
  {        
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrName)]
    public string name;
  }
}
