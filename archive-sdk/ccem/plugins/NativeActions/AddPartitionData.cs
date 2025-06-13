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
    public class PopulatePartitionData : INativeAction
    {
        [ActionParamField("AXL SOAP Partition Response ", "AXL SOAP Partition Response", true)]
        public object AxlPartitionResponse { set { partitionResponse = value; } }
        private object partitionResponse;

        [ResultDataField("")]
        public object PartitionData { get { return partitionData; } }
        private object partitionData;

        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        public void Clear()
        {
            partitionData = null;
            partitionResponse = null;
        }

        public PopulatePartitionData()
        {
            Clear();    
        }
        
        public bool ValidateInput()
        {
            return true;
        }

       [Action("PopulatePartitionData", false, "Populate Partition Data", "Uses the response from AXL GetPartition and populates the PhoneInfo data structure")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            partitionData = partitionResponse;
            return IApp.VALUE_SUCCESS;
        }
    }

}
