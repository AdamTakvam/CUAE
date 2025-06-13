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
    public class PopulateCssData : INativeAction
    {
        [ActionParamField("AXL SOAP CSS Response ", "AXL SOAP CSS Response", true)]
        public object AxlCssResponse { set { cssResponse = value; } }
        private object cssResponse;

        [ResultDataField("")]
        public object CssData { get { return cssData; } }
        private object cssData;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            cssData = null;
            cssResponse = null;
        }

        public PopulateCssData()
        {
            Clear();    
        }
        
        public bool ValidateInput()
        {
            return true;
        }

       [Action("PopulateCssData", false, "Populate Css Data", "Uses the response from AXL GetCss and populates the PhoneInfo data structure")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            cssData = cssResponse;
            return IApp.VALUE_SUCCESS;
        }
    }

}
