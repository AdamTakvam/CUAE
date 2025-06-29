using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using ReturnValues = Metreos.ApplicationSuite.Storage.ScheduledConferences.AddToQueueReturnValues;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary>Adds a file to the queue</summary>
	public class AddToQueue: INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("ScheduledConference record ID.", true)]
        public  uint ScheduledConferenceId { set { scheduledConferenceId = value; } }
        private uint scheduledConferenceId;

		[ActionParamField("1 for entry into conference, 2 for exiting conference")]
		public	uint AnnouncementType { get { return announcementType; } }
		private uint announcementType;
		
		[ActionParamField("Additional file to queue with incoming call")]
		public	string Filename { get { return filename; } }
		private string filename;
        
        [ResultDataField("The key of this queue item")]
        public  uint QueueId { get { return queueId; } }
        private uint queueId;

		public AddToQueue()
		{
			Clear();
		}

		#region INativeAction Implementation

		public bool ValidateInput()
		{
			return true;
		}

		public void Clear()
		{
            filename = null;
			scheduledConferenceId = announcementType = queueId = 0;			 
		}

        [ReturnValue(typeof(ReturnValues), "PlayFile means that the queue was empty and no files were being played, so proceed with play.  The file will be added to the queue")]
		[Action("AddToQueue", false, "Add To Queue", "Adds to a queue of outstanding filenames to play, unless the a file is not playing currently")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            using(ScheduledConferences schedConf = new ScheduledConferences(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ReturnValues result = schedConf.AddToQueue(
                    scheduledConferenceId, 
                    announcementType, 
                    filename,
                    out queueId);

                return result.ToString();
            }
        }

		#endregion
	}	// class GetConference
}
