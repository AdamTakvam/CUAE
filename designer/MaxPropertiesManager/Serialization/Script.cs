using System.Xml.Serialization;
using Metreos.ApplicationFramework.ScriptXml;

namespace Metreos.Max.Framework.Satellite.Property 
{ 
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)]
  public class MaxScriptType
  {        
    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrDescription)]
    public string description;
  }
}
