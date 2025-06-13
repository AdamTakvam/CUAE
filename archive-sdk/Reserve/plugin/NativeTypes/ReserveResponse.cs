using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;

using Metreos.Common.Reserve;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Types.Reserve
{
	public class ReserveResponse : IVariable
	{
        public static XmlSerializer seri = new XmlSerializer(typeof(ReserveResponseType));

        public string ResultCode { get { return reserveResponse.ResultCode; } set { reserveResponse.ResultCode = value; } }
        public string ResultMessage { get { return reserveResponse.ResultMessage; } set { reserveResponse.ResultMessage = value; } }
        public string DiagnosticCode { get { return reserveResponse.DiagnosticCode; } set { reserveResponse.DiagnosticCode = value; } }
        public string DiagnosticMessage { get { return reserveResponse.DiagnosticMessage; } set { reserveResponse.DiagnosticMessage = value; } }

        public string LoggedInUser { get { return reserveResponse.LoggedInUser; } set { reserveResponse.LoggedInUser = value; } }
        public string LoggedInDevice { get { return reserveResponse.LoggedInDevice; } set { reserveResponse.LoggedInDevice = value; } }


        private ReserveResponseType reserveResponse;

		public ReserveResponse()
		{
			reserveResponse = new ReserveResponseType();
            reserveResponse.ResultCode = ((int)Messaging.ResultCodes.Success).ToString();
            reserveResponse.ResultMessage = Messaging.ResultMessages.SuccessMessage;
		}
        
        public bool Parse(string defaultValue)
        {
            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new System.IO.StringWriter(builder);
            seri.Serialize(writer, reserveResponse);
            writer.Close();
            return writer.ToString();
        }
	}
}