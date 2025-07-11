﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=1.1.4322.573.
// 
namespace Metreos.Providers.CiscoDeviceListX {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public class DeviceList {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Device", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeviceListDevice[] Items;

        [System.Xml.Serialization.XmlElementAttribute("Error", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Error Error;

        [System.Xml.Serialization.XmlIgnore()]
        public bool IsError { get { return Error != null; } }
    }
    
    public class Error
    {
        [System.Xml.Serialization.XmlAttributeAttribute("number", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string number;

        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    /// <remarks/>
    public class DeviceListDevice {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("t")]
        public string type;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("n")]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("d")]
        public string description;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("c")]
        public string css;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("p")]
        public string pool;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("i")]
        public string ip;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("s")]
        public string status;

        /// <summary> 
        ///     This class is a serialization class, reflecting the 
        ///     XML retrieved from DeviceListX.  However, since we also use it as a data
        ///     store when retrieving from the database, we also have this field
        ///     which reflects our DB record, but not the XML
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public string ccmIP;

        /// <summary>
        ///     Combines devicename and ccmip to serve as a 
        ///     unique identifier for this record.  This is
        ///     necessary because we aggregate data from multiple
        ///     call managers, so there is a potential for 
        ///     the same devicename to occur in multiple ccms
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public string ID { get { return Metreos.Interfaces.ICiscoDeviceList.Device.FormatID(name, ccmIP); } }

        public string MakeID(string ccmIP)
        {
            return Metreos.Interfaces.ICiscoDeviceList.Device.FormatID(name, ccmIP);
        }

        public static bool SplitID(string ID, out string ccmIP, out string deviceName)
        {
            ccmIP = null;
            deviceName = null;
            bool success = false;

            if(ID != null)
            {
                string[] bits = ID.Split(seperator);

                if(bits.Length == 2)
                {
                    deviceName  = bits[0];
                    ccmIP       = bits[1];
                    success = true;
                }
            }
        
            return success;
        }

        private static char[] seperator = new char[] {'@'};
    }
}
