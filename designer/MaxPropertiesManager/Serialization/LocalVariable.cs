using System.Xml.Serialization;
 
namespace Metreos.Max.Framework.Satellite.Property 
{
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Variable.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)] //, Namespace="http://tempuri.org/Variable.xsd", IsNullable=false)]
  public class MaxLocalVariableType 
  {
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrInitWith)]
    public string initWith;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrName)]
    public string eventName;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrDefaultInitWith)]
    public string defaultInitWith;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrType)]
    public string varType;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrRefType)]
    public DataTypes.ReferenceType referenceType;


    /// <summary> Stores the name of the variable </summary>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
  }
}