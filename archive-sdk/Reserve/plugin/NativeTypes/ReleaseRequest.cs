using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Metreos.Common.Reserve;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Types.Reserve
{
	public class ReleaseRequest : IVariable
	{
        public static XmlSerializer seri = new XmlSerializer(typeof(ReleaseRequestType));

        public string DeviceName { get { return releaseRequest.DeviceName; } }
        public string CcmUser { get { return releaseRequest.CcmUser; } }
        public string IntegrationUser { get { return releaseRequest.IntegrationUser; } }
        public string RecordId { get { return releaseRequest.RecordId; } }
        public string SecurityToken { get { return releaseRequest.SecurityToken; } }
        
        private ReleaseRequestType releaseRequest;

		public ReleaseRequest()
		{
			releaseRequest = new ReleaseRequestType();
        }
        
        public bool Parse(string defaultValue)
        {
            StringReader reader = new StringReader(defaultValue);
            releaseRequest = seri.Deserialize(reader) as ReleaseRequestType;
            return true;
        }

        public override string ToString()
        {
            StringWriter writer = new System.IO.StringWriter();
            seri.Serialize(writer, releaseRequest);
            writer.Close();
            return writer.ToString();
        }
	}
}
