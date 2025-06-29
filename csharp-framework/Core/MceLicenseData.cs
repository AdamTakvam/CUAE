using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Metreos.Core
{
    /// <summary>
    /// Represents the raw data for an MCE license.
    /// </summary>
    /// 
    /// <example>
    /// A sample license is shown below:
    /// 
    ///    <?xml version="1.0" encoding="utf-16"?>
    ///    <metreosLicense xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    ///       <created>Wednesday, October 01, 2003</created>
    ///       <expires>Thursday, January 01, 2004</expires>
    ///       <serialNumber>e0d72998-4c88-4a96-92a4-19c8ad33b8ea</serialNumber>
    ///       <signature>UGu87f9+lGhbH5CB/CMrkgfsWTRZ3h+gBCkecbYR2iz+P63cU8S5pQ==</signature>
    ///       <numRunningInstances>50</numRunningInstances>
    ///       <numApplicationTypes>5</numApplicationTypes>
    ///       <numLoadedProviders>10</numLoadedProviders>
    ///    </metreosLicense>
    ///    
    /// </example>
    [XmlRootAttribute(ElementName = "metreosLicense", IsNullable = false)]
    public class MceLicenseData
    {
        private string creationDateTime;
        private string expirationDateTime;
        private string serialNumber;
        private string signature;
        private int numRunningInstances;
        private int numApplicationTypes;
        private int numLoadedProviders;

        [XmlElement(ElementName = "created")]
        public string CreationDateTime
        {
            get { return creationDateTime; }
            set { creationDateTime = value; }
        }


        [XmlElement(ElementName = "expires")]
        public string ExpirationDateTime
        {
            get { return expirationDateTime; }
            set { expirationDateTime = value; }
        }


        [XmlElement(ElementName = "serialNumber")]
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }


        [XmlElement(ElementName = "signature")]
        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }


        [XmlElement(ElementName = "numRunningInstances")]
        public int NumRunningInstances
        {
            get { return numRunningInstances; }
            set { numRunningInstances = value; }
        }


        [XmlElement(ElementName = "numApplicationTypes")]
        public int NumApplicationTypes
        {
            get { return numApplicationTypes; }
            set { numApplicationTypes = value; }
        }


        [XmlElement(ElementName = "numLoadedProviders")]
        public int NumLoadedProviders
        {
            get { return numLoadedProviders; }
            set { numLoadedProviders = value; }
        }


        public override string ToString()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MceLicenseData));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.IO.TextWriter writer = new System.IO.StringWriter(sb);
            serializer.Serialize(writer, this);
            writer.Close();

            serializer = null;
            writer = null;

            return sb.ToString();
        }


        public byte[] GetByteData()
        {
            return System.Text.Encoding.Default.GetBytes(this.ToString());
        }

    }
}
