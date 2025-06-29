using System;
using System.Collections;
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
    public class GetVoiceMailRecords : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("userId of user whose VoiceMail records we wish to retrieve", true)]
        public uint UserId { set { userId = value; } }
        private uint userId;

        [ActionParamField("Specifies type of messages that we want to retrieve. Valid types are: All, New, Old, Saved, Deleted, Flagged, Other", true)] 
        public string Status { set { status = value; } }
        private string status;

        [ActionParamField("Specifies the order in which we want the collection to be maintained. Valid values are: Increasing, Decreasing. Default: Increasing", false)] 
        public string SortingOrder { set { sortingOrder = value; } }
        private string sortingOrder;

        [ResultDataField("VoiceMailRecordCollection containing VoiceMail records for given user")]
        public VoiceMailRecordCollection RecordCollection { get { return recordCollection; } }
        private VoiceMailRecordCollection recordCollection;

        [ResultDataField("Number of returned records")]
        public int RecordCount { get { return recordCount; } }
        private int recordCount;

        public GetVoiceMailRecords()
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
            sortingOrder = null;
        }

        [Action("GetVoiceMailRecords", false, "GetVoiceMailRecords", "Retrieves VoiceMail records associated with given user and given status.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(VoiceMails vmDbAccess = new VoiceMails(
                      sessionData.DbConnections[SqlConstants.DbConnectionName], 
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
                if ( vmDbAccess.GetVoiceMailRecords(userId, out recordCollection, status) )
                {
                    try
                    {
                        recordCollection.SortingOrder = (SortOrder) Enum.Parse(typeof(SortOrder), sortingOrder, true);
                    }
                    catch
                    {
                        recordCollection.SortingOrder = SortOrder.Increasing;
                    }
                
                    recordCount = recordCollection.Count;
                    recordCollection.Sort();
                
                    return IApp.VALUE_SUCCESS;
                }
            
                return IApp.VALUE_FAILURE;
            }
        }
    }
}
