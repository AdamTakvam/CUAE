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
    public class PopulateLineData : INativeAction
    {
        [ActionParamField("AXL SOAP Line Response ", "AXL SOAP Line Response", true)]
        public object AxlLineResponse { set { lineResponse = value; } }
        private object lineResponse;

        [ResultDataField("")]
        public object LineData { get { return lineData; } }
        private object lineData;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            lineData = null;
            lineResponse = null;
        }

        public PopulateLineData()
        {
            Clear();    
        }
        
        public bool ValidateInput()
        {
            return true;
        }

       [Action("PopulateLineData", true, "Populate Line Data", "Uses the response from AXL GetLine and populates the PhoneInfo data structure")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            lineData = lineResponse;
            return IApp.VALUE_SUCCESS;
        }
    }

}
