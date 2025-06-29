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
    /// Retrieves voicemail records associated with a user that have the specified status.
    /// </summary>
    public class CreateVoiceMailRecord : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("UserId of user with whom this record will be associated", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("Time stamp for the message", true)]
        public DateTime TimeStamp { set { timeStamp = value; } }
        private DateTime timeStamp;

        [ActionParamField("Specifies type of the message we are creating. Valid types are: All, New, Old, Saved, Deleted, Flagged, Other", true)]
        public string Status { set { status = value; } }
        private string status;

        [ActionParamField("Length of the message in seconds.", true)]
        public uint Length { set { length = value; } }
        private uint length;

        [ResultDataField("VoiceMailRecord that was created by this action.")]
        public VoiceMailRecord Record { get { return record; } }
        private VoiceMailRecord record;

        // Trying a different approach where filename is automatically generated and returned. 
        [ResultDataField("Filename for the recorded message")]
        public string Filename { set { filename = value; } }
        private string filename;
        
        public CreateVoiceMailRecord()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return (userId >= SqlConstants.StandardPrimaryKeySeed) && (status != null);
        }

        public void Clear()
        {
            userId = 0;
            status = null;
            filename = null;
            length = 0;
            record = null;
        }

        [Action("CreateVoiceMailRecord", false, "CreateVoiceMailRecord", "Creates a VoiceMail record and returns the object into the variable specified in the ResultData field")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            try 
            {
                uint msgStatus;
                if ( ! VoiceMails.StatusStringToUInt(status, out msgStatus))
                    throw new Exception("Invalid message status specified: " + status);

                record = new VoiceMailRecord(userId, filename, msgStatus, length, timeStamp);
                record.IsNew = true;
                filename = record.Filename = userId.ToString() + "-" + System.DateTime.Now.ToLongDateString();

                return IApp.VALUE_SUCCESS;
            }
            catch (Exception e)
            {
                log.Write(TraceLevel.Warning, "Exception thrown while creating record " +
                                              "object in CreateVoiceMailRecord action. Exception message is:\n {0}",
                                                e.Message);
                return IApp.VALUE_FAILURE;
            }
        }
    }
}
