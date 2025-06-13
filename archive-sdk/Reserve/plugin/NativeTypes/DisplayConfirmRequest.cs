using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Xml.Serialization;

using Metreos.Common.Reserve;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Types.Reserve
{
	public class DisplayConfirmRequest : IVariable
	{
        public static XmlSerializer seri = new XmlSerializer(typeof(DisplayRequestType));

        private DisplayRequestType displayConfirmRequest;

        public int PollMinutes { get { return displayConfirmRequest.pollMinutes; } }
        public DisplayItem[] Items { get { return displayConfirmRequest.items; } }

		public DisplayConfirmRequest()
		{
			displayConfirmRequest = new DisplayRequestType();
        }
        
        public bool Parse(string defaultValue)
        {
            StringReader reader = new StringReader(defaultValue);
            displayConfirmRequest = seri.Deserialize(reader) as DisplayRequestType;
            return true;
        }

        public override string ToString()
        {
            StringWriter writer = new System.IO.StringWriter();
            seri.Serialize(writer, displayConfirmRequest);
            writer.Close();
            return writer.ToString();
        }
	}
}
