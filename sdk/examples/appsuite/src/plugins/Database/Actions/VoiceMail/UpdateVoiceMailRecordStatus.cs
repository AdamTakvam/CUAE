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
    /// Updates the status for a given VoiceMailRecord
    /// </summary>
    public class UpdateVoiceMailRecordStatus : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Record that we wish to update", true)]
        public VoiceMailRecord Record { set { record = value; } }
        private VoiceMailRecord record;

        [ActionParamField("Specifies new status for specified record. Valid types are: All, New, Old, Saved, Deleted, Flagged, Other", true)] 
        public string Status { set { status = value; } }
        private string status;

        public UpdateVoiceMailRecordStatus()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return (record != null) && (status != null);
        }

        public void Clear()
        {
            record = null;
            status = null;
        }

        [Action("UpdateVoiceMailRecordStatus", false, "UpdateVoiceMailRecordStatus", "Updates the status of a specified VoiceMail record")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            uint recordStatus;
            if (VoiceMails.StatusStringToUInt(status, out recordStatus))
            {
                
                record.Status = recordStatus;
                return IApp.VALUE_SUCCESS;
            }
 
            return IApp.VALUE_FAILURE;
        }
    }
}
