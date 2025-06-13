using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Common.Ccem;

namespace Metreos.Types.Ccem
{
    public class PhoneData : IVariable
    {
        public static XmlSerializer seri = new XmlSerializer(typeof(PhoneInfo));

        public PhoneInfo Data { get { return phoneInfo; } } 
        private PhoneInfo phoneInfo;

        public PhoneData()
        {
            phoneInfo = new PhoneInfo();
        }
        
        public bool Parse(Metreos.AxlSoap413.getPhoneResponse phone)
        {
            phoneInfo.SetDevice(phone);
            return true;
        }

        public bool Parse(Metreos.AxlSoap413.getLineResponse line)
        {
            phoneInfo.AddLineInfo(line);
            return true;
        }

        public bool Parse(string str)
        {
            return false;
        }

        public override string ToString()
        {
            StringWriter writer = new System.IO.StringWriter();
            seri.Serialize(writer, phoneInfo);
            writer.Close();
            return writer.ToString();
        }
    }
}
