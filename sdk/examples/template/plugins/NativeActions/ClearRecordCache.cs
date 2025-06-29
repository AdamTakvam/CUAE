using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections;
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
    /// <summary> Deletes the temporary record file, and saves those items to the database </summary>
    [PackageDecl("Metreos.Native.CallMonitor")]
    public class ClearRecordCache : INativeAction
    {
        public LogWriter Log { set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Temporary File Path", true)]
        public string TemporaryFilePath { set { temporaryFilePath = value; } }
        private string temporaryFilePath;
 
        [ActionParamField("Data Source Name", true)]
        public string DSN { set { dsn = value; } }
        private string dsn;

        public ClearRecordCache()
        {
            Clear();
        }
 
        public void Clear()
        {
            temporaryFilePath       = null;
            dsn                     = null;
        }

        public bool ValidateInput()
        {
            return true;
        }
 
        [Action("ClearRecordCache", false,"Clear Record Cache", "Deletes the temporary record file, and saves those items to the database.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            bool success = true;

            CallRecordTable records = CallRecordTable.RetrieveRecords(temporaryFilePath, log);
          
            try
            {
                File.Delete(temporaryFilePath);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, 
                    "Unable to delete temporary record file at '{0}'!  " + 
                    "No records will be cleared from the file, and the database records will not be written " +
                    "(The application will not back up records to the database unless it can delete the back up file, to avoid duplicate entries.)\n" + 
                    Exceptions.FormatException(e), temporaryFilePath);
            }

            CallRecordTable failedWrites = CallRecordTable.WriteToDatabase( dsn, records, log);

            if(failedWrites != null && failedWrites.Records != null && failedWrites.Records.Length > 0)
            {
                bool backupRecordsSuccess = CallRecordTable.WriteRecords(temporaryFilePath, failedWrites, log);

                if(!backupRecordsSuccess)
                {
                    success = false;
                    StringBuilder sb = new StringBuilder();
                    TextWriter writer = new StringWriter(sb);
                    CallRecordTable.Seri.Serialize(writer, failedWrites);

                    log.Write(TraceLevel.Error, 
                        "Unable to save to disk temporary records which also failed to save to database.  " + 
                        "Some records will be lost.  Outputing these records to the log as best-effort attempt.\n  " + 
                        sb.ToString());
                }
            }
            
            return success ? IApp.VALUE_SUCCESS : IApp.VALUE_FAILURE;
        }
    }
}
