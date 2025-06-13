using System;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

namespace Metreos.Native.VendorDb
{
	/// <summary>
	///     Given the IP address of a Cisco IP Phone, retrieves the DeviceInformation  XML.
	///     Something like this easily belongs is the framework, but anything of this nature
	///     should be tested with all models of all IP phones, as well as across all versions of CCM.
	/// </summary>
	public class GetDeviceInformation : INativeAction
	{
        private static XmlSerializer deseri = new XmlSerializer(typeof(DeviceInformation));

        [ActionParamField("Phone IP", true)]
        public string PhoneIP { set { phoneIp = value; } }
        private string phoneIp;

        [ResultDataField("PhoneDN")]
        public string PhoneDN { get { return phoneDn; } }
        private string phoneDn;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            phoneIp = null;
            phoneDn = null;
        }

		public GetDeviceInformation()
		{
		    Clear();    
		}

        
        public bool ValidateInput()
        {
            return true;
        }

        [ReturnValue(typeof(UrlStatus), "The result of HTTP GET to the phone")]
        [Action("GetDeviceInformation", false, "Get Device Info", "Returns the Device Information retrieved from a phone")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            object deviceInfoObj;
            UrlStatus status = Web.XmlDeserialize(String.Format("http://{0}/DeviceInformationX", phoneIp), deseri, out deviceInfoObj);
            DeviceInformation deviceInfo = deviceInfoObj as DeviceInformation;
            if(deviceInfo != null)
            {
                phoneDn = deviceInfo.phoneDN;
            }
            return status.ToString();
        }
	}

    [Serializable()]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class DeviceInformation 
    {
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MACAddress;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string HostName;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phoneDN;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appLoadID;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bootLoadID;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string versionID;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addonModule1;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addonModule2;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hardwareRevision;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serialNumber;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string modelNumber;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Codec;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Amps;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string C3PORevision;
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageWaiting;
    }
}
