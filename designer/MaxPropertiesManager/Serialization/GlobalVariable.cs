using System.Xml.Serialization;
 
namespace Metreos.Max.Framework.Satellite.Property 
{
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Variable.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)] //, Namespace="http://tempuri.org/Variable.xsd", IsNullable=false)]
  public class MaxGlobalVariableType 
  {
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrInitWith)]
    public string initWith;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrDefaultInitWith)]
    public string defaultInitWith;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrType)]
    public string varType;

    /// <summary> Stores the name of the variable </summary>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
  }
}