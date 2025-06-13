using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Metreos.Common.Reserve;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Types.Reserve
{
	public class ReleaseResponse : IVariable
	{
        public static XmlSerializer seri = new XmlSerializer(typeof(ReleaseResponseType));

        public string ResultCode { get { return releaseResponse.ResultCode; } set { releaseResponse.ResultCode = value; } }
        public string ResultMessage { get { return releaseResponse.ResultMessage; } set { releaseResponse.ResultMessage = value; } }
        public string DiagnosticCode { get { return releaseResponse.DiagnosticCode; } set { releaseResponse.DiagnosticCode = value; } }
        public string DiagnosticMessage { get { return releaseResponse.DiagnosticMessage; } set { releaseResponse.DiagnosticMessage = value; } }

        private ReleaseResponseType releaseResponse;

		public ReleaseResponse()
		{
			releaseResponse = new ReleaseResponseType();
            releaseResponse.ResultCode = ((int) Messaging.ResultCodes.Success).ToString();
            releaseResponse.ResultMessage = Messaging.ResultMessages.SuccessMessage;
		}
        
        public bool Parse(string defaultValue)
        {
            return false;
        }

        public override string ToString()
        {
            StringWriter writer = new System.IO.StringWriter();
            seri.Serialize(writer, releaseResponse);
            writer.Close();
            return writer.ToString();
        }
	}
}
