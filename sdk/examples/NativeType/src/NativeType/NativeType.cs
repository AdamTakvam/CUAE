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

namespace Metreos.Native.MyTypes
{
    /// <summary>
    ///     Given the IP address of a Cisco IP Phone, retrieves the DeviceInformation  XML.
    ///     Something like this easily belongs is the framework, but anything of this nature
    ///     should be tested with all models of all IP phones, as well as across all versions of CCM.
    /// </summary>
    public class NativeType : IVariable
    {
        public string getHour { get { return hour; } }
        private string hour;
        public string getMin { get { return minutes; } }
        private string minutes;
        public string getSec { get { return seconds; } }
        private string seconds;

        public LogWriter Log { get { return log; } set { log = value; } }
        private LogWriter log;

        public NativeType() 
        { 
        }

        public bool Parse(string newValue)
        {            
            string[] myValues = newValue.Split(':');
            hour = myValues[0];
            minutes = myValues[1];
            seconds = myValues[2];

            return true;
        }

        public bool Parse(object nonStringType)
        {
            return true;
        }    
    }
}
