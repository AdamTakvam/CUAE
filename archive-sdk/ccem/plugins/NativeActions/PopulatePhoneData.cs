using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.Native.Ccem
{
    /// <summary>
    /// </summary>
    public class PopulatePhoneData : INativeAction
    {
        [ActionParamField("AXL SOAP Phone Response ", "AXL SOAP Phone Response", true)]
        public object AxlPhoneResponse { set { phoneResponse = value; } }
        private object phoneResponse;

        [ResultDataField("")]
        public object PhoneData { get { return phoneData; } }
        private object phoneData;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            phoneData = null;
            phoneResponse = null;
        }

        public PopulatePhoneData()
        {
            Clear();    
        }
        
        public bool ValidateInput()
        {
            return true;
        }

        [Action("PopulatePhoneData", true, "Populate Phone Data", "Uses the response from AXL GetPhone and populates the PhoneInfo data structure")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            phoneData = phoneResponse;
            return IApp.VALUE_SUCCESS;
        }
    }

}
