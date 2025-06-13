using Metreos.ApplicationFramework.ScriptXml;
using System.Xml.Serialization;
 
namespace Metreos.Max.Framework.Satellite.Property 
{
    
    
  /// <remarks/>
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Loop.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)] //, Namespace="http://tempuri.org/Loop.xsd", IsNullable=false)]
  public class MaxLoopType 
  {
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrLoopIteratorType)]
    public loopCountEnumType loopIteratorType;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrType)]
    public paramType type;

    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
  }
}
