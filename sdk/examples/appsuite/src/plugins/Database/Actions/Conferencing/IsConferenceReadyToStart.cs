using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using ReturnValues = Metreos.ApplicationSuite.Storage.ScheduledConferences.IsConferenceReadyToStartReturnValues;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary>Validate conference properties for (scheduled) conference ID</summary>
	public class IsConferenceReadyToStart : INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("Participant conference PIN.", true)]
		public  uint ConferencePin { set { conferencePin = value; } }
		private uint conferencePin;

        [ActionParamField("The amount of minutes the conferene can start before the scheduled time.", true)]
        public  uint Tolerance { set { tolerance = value; } }
        private uint tolerance;

		public IsConferenceReadyToStart()
		{
			Clear();
		}

		public bool ValidateInput()
		{
            return true;
		}

		public void Clear()
		{
			conferencePin  = 0;		
	        tolerance = 0;
        }


        [ReturnValue(typeof(ReturnValues), "Indicates if the conference is ready to start")]
        [Action("IsConferenceReadyToStart", false, "Is Conference Ready", "Validate conference properties.")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            using(ScheduledConferences schedConf = new ScheduledConferences(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {
            
                DateTime scheduledTime;
                uint durationMinutes = 0;

                uint dummy;
                bool dummyBool;

                schedConf.GetConferenceInfo(
                    conferencePin, 
                    out dummy, 
                    out dummy,
                    out dummy,
                    out scheduledTime,
                    out durationMinutes,
                    out dummy,
                    out dummyBool,
                    out dummy);

                if(durationMinutes == 0)
                {
                    return ReturnValues.Failure.ToString();
                }

                if (DateTime.Now < scheduledTime.AddMinutes(-1 * tolerance))
                {
                    return ReturnValues.No.ToString();
                }
                else if (DateTime.Now > scheduledTime.AddMinutes(durationMinutes))
                {
                    return ReturnValues.Expired.ToString();
                }

                return ReturnValues.Yes.ToString();
            }
		}
	}	
}
