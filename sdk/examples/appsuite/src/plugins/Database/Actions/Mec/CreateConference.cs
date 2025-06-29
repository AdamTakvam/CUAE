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
using ResultValues = Metreos.ApplicationSuite.Storage.MecConferences.ResultValues;

namespace Metreos.ApplicationSuite.Actions
{
    /// <summary>Creates a conference record
    /// </summary>
    public class CreateConference : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ConferenceId", true)]
        public string SessionId { set { sessionId = value; } }
        private string sessionId;

        [ResultDataField()]
        public int ConferenceId { get { return id; } }
        private int id;

        public CreateConference()
        {
            Clear();
        }
 
        public void Clear()
        {
            sessionId = null;
            id = 0;
        }
 
        public bool ValidateInput()
        {
            return true;
        }
 
        [ReturnValue(typeof(ResultValues), "")]
        [Action("CreateConference", false, "Success/Failure", "Creates a conference record")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(MecConferences conferenceDb = new MecConferences(
                      sessionData.DbConnections[SqlConstants.MecConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ResultValues result = conferenceDb.CreateConferenceRecord(sessionId, out id);

                return result.ToString();
            }
        }
    }
}
