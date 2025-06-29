using System;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationSuite.Storage;
using ReturnValues = Metreos.ApplicationSuite.Storage.ScheduledConferences.RemoveFromQueueReturnValues;

namespace Metreos.ApplicationSuite.Actions
{
	/// <summary>Removes a filename-queue entry, and returns the next one to play</summary>
	public class RemoveFromQueue: INativeAction
	{
        public LogWriter Log { get { return log; } set { log = value; } } 
        private LogWriter log;

        [ActionParamField("The key of this queue item", true)]
        public  uint QueueId { set { queueId = value; } }
        private uint queueId;

        [ActionParamField("Participant conference PIN.", true)]
        public  uint ScheduledConferenceId { set { scheduledConferenceId = value; } }
        private uint scheduledConferenceId;

        [ResultDataField("The next key of this queue item")]
        public  uint NextQueueId { get { return nextQueueId; } }
        private uint nextQueueId;

        [ResultDataField("The type of the next announcement")]
        public  uint NextType { get { return nextType; } }
        private uint nextType;

        [ResultDataField("The filename of the next announcement. Is String.Empty if not defined")]
        public  string NextFilename { get { return nextFilename; } }
        private string nextFilename;
        
		public RemoveFromQueue()
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
            nextFilename = String.Empty;
			scheduledConferenceId = queueId = nextQueueId = nextType = 0;			 
		}

        [ReturnValue(typeof(ReturnValues), "NoNextFile means that the queue is empty")]
		[Action("RemoveFromQueue", false, "Remove From Queue", "Removes a file record from the queue, returning the next one to play")]
		public string Execute(SessionData sessionData, IConfigUtility configUtility)
		{
            using(ScheduledConferences schedConf = new ScheduledConferences(
                      sessionData.DbConnections[SqlConstants.DbConnectionName],
                      log,
                      sessionData.AppName,
                      sessionData.PartitionName,
                      DbTable.DetermineAllowWrite(sessionData.CustomData)))
            {

                ReturnValues result = schedConf.RemoveFromQueue(
                    scheduledConferenceId, 
                    queueId,
                    out nextFilename,
                    out nextQueueId,
                    out nextType);

                return result.ToString();
            }
        }

		#endregion
	}	// class GetConference
}
