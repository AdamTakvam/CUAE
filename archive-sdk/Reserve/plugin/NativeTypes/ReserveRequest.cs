using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Metreos.Common.Reserve;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Types.Reserve
{
	public class ReserveRequest : IVariable
	{
        public static XmlSerializer seri = new XmlSerializer(typeof(ReserveRequestType));

        public string CcmIP { get { return reserveRequest.CcmIP; } }
        public string CcmUser { get { return reserveRequest.CcmUser; } }
        public string DeviceName { get { return reserveRequest.DeviceName; } }
        public string DeviceProfile { get { return reserveRequest.DeviceProfile; } }
        public string First { get { return reserveRequest.First; } }
        public string Last { get { return reserveRequest.Last; } }
        public string IntegrationUser { get { return reserveRequest.IntegrationUser; } }
        public string RecordId { get { return reserveRequest.RecordId; } }
        public string SecurityToken { get { return reserveRequest.SecurityToken; } }
        public string Timeout { get { return reserveRequest.Timeout; } }
        private ReserveRequestType reserveRequest;

		public ReserveRequest()
		{
			reserveRequest = new ReserveRequestType();
        }
        
        public bool Parse(string defaultValue)
        {
            StringReader reader = new StringReader(defaultValue);
            reserveRequest = seri.Deserialize(reader) as ReserveRequestType;
            return true;
        }

        public override string ToString()
        {
            StringWriter writer = new System.IO.StringWriter();
            seri.Serialize(writer, reserveRequest);
            writer.Close();
            return writer.ToString();
        }
	}
}
