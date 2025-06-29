using System;
using System.Data;
using System.Diagnostics;
using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using Metreos.ApplicationSuite.Storage;
using ResultValues = Metreos.ApplicationSuite.Storage.MecConferences.ResultConferenceUpdate;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Starts a conference record
    /// </summary>
    public class StartConference : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ConferenceId", true)]
        public int ConferenceId { set { conferenceId = value; } }
        private int conferenceId;

        [ActionParamField("MmsConferenceId", true)]
        public string MmsConferenceId { set { mmsConferenceId = value; } }
        private string mmsConferenceId;

        public StartConference()
        {
            Clear();
        }
 
        public void Clear()
        {
            conferenceId = 0;
            mmsConferenceId = null;
        }
 
        public bool ValidateInput()
        {
            return true;
        }
 
        [ReturnValue(typeof(ResultValues), "")]
        [Action("StartConference", false, "Success/Failure/NoRecord", "Initialize conference record to real-world starting conditions")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(MecConferences conferenceDb = new MecConferences(
                      sessionData.DbConnections[SqlConstants.MecConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ResultValues result = conferenceDb.StartConference(conferenceId, mmsConferenceId);

                return result.ToString();
            }
        }
    }
}
