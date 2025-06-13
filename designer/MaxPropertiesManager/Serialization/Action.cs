using System.Xml.Serialization;
using Metreos.ApplicationFramework.ScriptXml;
 
namespace Metreos.Max.Framework.Satellite.Property 
{
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Action.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)] //, Namespace="http://tempuri.org/Action.xsd", IsNullable=false)]
  public class MaxActionType 
  {  
    [System.Xml.Serialization.XmlElementAttribute(Defaults.xmlEltActionParameter)]
    public actionParamType[] @params;
        
    [System.Xml.Serialization.XmlElementAttribute(Defaults.xmlEltResultData)]
    public resultDataType[] results;
        
    [System.Xml.Serialization.XmlElementAttribute(Defaults.xmlEltLogging)]
    public LogType[] logging;
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrLog)]
    public string log;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrFinal)]
    public bool final;
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrType)]
    public Metreos.PackageGeneratorCore.PackageXml.actionTypeType type;
  }

    
  //[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/Action.xsd")]
  public class LogType 
  {   
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrCondition)]
    public string condition;
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrLevel)]
    public System.Diagnostics.TraceLevel level;
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrOn)]
    public bool on;
        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrType)]
    public paramType type;

    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;
  } 
}
