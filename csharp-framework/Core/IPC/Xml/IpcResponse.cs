using System.Xml.Serialization;

using Metreos.Interfaces;
using Metreos.Core.ConfigData;

namespace Metreos.Core.IPC.Xml
{
    [System.Xml.Serialization.XmlRootAttribute("response", IsNullable=false)]
	public class responseType
	{
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public IConfig.Result type;

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value;

        [System.Xml.Serialization.XmlElementAttribute("componentInfo")]
        public ComponentInfo[] componentInfo;

        [System.Xml.Serialization.XmlElementAttribute("resultList")]
        public string[] resultList;
	}
}
