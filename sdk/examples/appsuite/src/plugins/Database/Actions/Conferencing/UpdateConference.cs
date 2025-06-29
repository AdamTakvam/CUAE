using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

using Metreos.ApplicationSuite.Storage;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary> Set conference media properties </summary>
    public class UpdateConference : INativeAction
    {
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Scheduled conference table key.", true)]
        public  uint ScheduledConferenceId { set { scheduledConferenceId = value; } }
        private uint scheduledConferenceId;

        [ActionParamField("Media server ID.")]
        public	uint MmsId { set { mmsId = value; } }
        private uint mmsId;
		
        [ActionParamField("Media server conference ID.")]
        public	uint MmsConfId	{ set { mmsConfId = value; } }
        private uint mmsConfId;


        public UpdateConference()
        {
            Clear();
        }

        public bool ValidateInput()
        {
            return true;
        }

        public void Clear()
        {
            scheduledConferenceId = mmsId = mmsConfId = 0;			 
        }

        [Action("UpdateConference", false, "Update Conference", "Set conference media properties.")]
        public string Execute(SessionData sessionData, IConfigUtility configUtility)
        {
            using(ScheduledConferences schedConf = new ScheduledConferences(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                if(schedConf.UpdateConference(scheduledConferenceId, mmsId, mmsConfId) == false)
                {
                    return IApp.VALUE_FAILURE;
                }

                return IApp.VALUE_SUCCESS;
            }
        }
	}	// class UpdateConference
}
