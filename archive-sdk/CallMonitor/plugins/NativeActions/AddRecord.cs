using System;
using System.IO;
using System.Data;
using System.Xml.Serialization;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;

namespace Metreos.Native.CallMonitor
{ 
    /// <summary> Adds a record to flat-file.  Intended for use if case db goes down. </summary>
    [PackageDecl("Metreos.Native.CallMonitor")]
    public class AddRecord : INativeAction
    {
        private static XmlSerializer seri = new XmlSerializer(typeof(CallRecordTable));

        public LogWriter Log { set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Government Agent Number", true)]
        public string GovAgentNumber { set { govAgentNumber = value; } }
        private string govAgentNumber;

        [ActionParamField("DID", true)]
        public string DID { set { did = value; } }
        private string did;

        [ActionParamField("Insurance Agent Number", true)]
        public string InsuranceAgentNumber { set { insuranceAgentNumber = value; } }
        private string insuranceAgentNumber;

        [ActionParamField("Customer Number", true)]
        public string CustomerNumber { set { customerNumber = value; } }
        private string customerNumber;

        [ActionParamField("Monitored Sid", true)]
        public string MonitoredSid { set { monitoredSid = value; } }
        private string monitoredSid;

        [ActionParamField("Temporary File Path", true)]
        public string TemporaryFilePath { set { temporaryFilePath = value; } }
        private string temporaryFilePath;
 
        public AddRecord()
        {
            Clear();
        }
 
        public void Clear()
        {
            temporaryFilePath       = null;
            govAgentNumber          = null;
            did                     = null;
            insuranceAgentNumber    = null;
            customerNumber          = null;
            monitoredSid            = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
        
        [Action("AddRecord", false,"Add Record", "Adds a record to flat-file.  Intended for use if case db goes down.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            bool success = false;
            
            CallRecordTable record = CallRecordTable.RetrieveRecords(temporaryFilePath, log);
           
            // Create the new record construction
            Record newRecord = new Record();
            newRecord.customerNumber = customerNumber;
            newRecord.did = did;
            newRecord.governmentAgentNumber = govAgentNumber;
            newRecord.insuranceAgentNumber = insuranceAgentNumber;
            newRecord.monitoredSid = monitoredSid;
            newRecord.startMonitorTime = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss:fff");

            if(record == null)
            {
                record = new CallRecordTable();
                record.Records = new Record[1]; // We initialize our record struction
                record.Records[0] = newRecord;
            }
            else
            {
                Record[] grownRecords = new Record[record.Records.Length + 1];
                record.Records.CopyTo(grownRecords, 0);
                grownRecords[grownRecords.Length - 1] = newRecord;
                record.Records = grownRecords;
            }

            success = CallRecordTable.WriteRecords(temporaryFilePath, record, log);

            return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
        }
    }
}
