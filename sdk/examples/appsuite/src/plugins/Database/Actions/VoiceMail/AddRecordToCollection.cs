using System;
using System.Collections;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using Metreos.ApplicationSuite.Types;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>
    /// Adds a VoiceMailRecord to a VoiceMailRecordCollection
    /// </summary>
    public class AddVoiceMailRecordToCollection : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Record that we wish to add.", true)]
        public VoiceMailRecord Record { set { record = value; } }
        private VoiceMailRecord record;
     
        [ActionParamField("Collection that we wish to add the record to.", true)]
        public VoiceMailRecordCollection RecordCollection { set { recordCollection = value; } }
        private VoiceMailRecordCollection recordCollection;

        [ResultDataField("Current count of elements in the collection.")]
        public int RecordCount { get { return recordCount; } }
        private int recordCount;

        public AddVoiceMailRecordToCollection()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return (record != null) && (recordCollection != null);
        }

        public void Clear()
        {
            record = null;
            recordCollection = null;
        }

        [Action("AddVoiceMailRecordToCollection", false, "AddVoiceMailRecordToCollection", "Adds a VoiceMailRecord to a VoiceMailRecordCollection.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(VoiceMails vmDbAccess = new VoiceMails(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                try
                {
                    recordCollection.Add(record);
                    recordCollection.Sort();
                    recordCount = recordCollection.Count;
                }
                catch (Exception e)
                {
                    Log.Write(TraceLevel.Error, "Could not add record to record collection.\nException: " + e.Message);
                    return IApp.VALUE_FAILURE;
                }

                return IApp.VALUE_SUCCESS;
            }
        }
    }
}
