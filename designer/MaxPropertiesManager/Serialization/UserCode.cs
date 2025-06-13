using System.Xml.Serialization;
 
namespace Metreos.Max.Framework.Satellite.Property 
{
  [System.Xml.Serialization.XmlRootAttribute(Defaults.xmlEltProperties)]
  public class MaxCodeType 
  {
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value;

    [System.Xml.Serialization.XmlAttributeAttribute(Defaults.xmlAttrLanguage)]
    public DataTypes.AllowableLanguages language;

    [System.Xml.Serialization.XmlElementAttribute(Defaults.xmlEltLogging)]
    public LogType[] logging;
  }
}