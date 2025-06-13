using System;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;

namespace Metreos.Native.CBridge
{
	public class Participant : IVariable
	{
		public string LineId { set { lineId = value; } }
		private string lineId;

		public string From { set { from = value; } }
		private string from;

		public string CallId { set { callId = value; } }
		private string callId;

		public DateTime Timestamp { set { timestamp = value; } }
		private DateTime timestamp;

		public bool IsRecorded { set { isRecorded = value; } }
		private bool isRecorded = false;
		
		public bool IsModerator { set { isModerator = value; } }
		private bool isModerator = false;

		public bool IsMuted { set { isMuted = value; } }
		private bool isMuted = false;

		public Participant()
		{
			lineId = from = callId = null;
		}

        public Participant(object participant)
        {
            this.Parse(participant);
        }
		
		#region IVariable Members

		public bool Parse(string str)
		{
			// TODO:  Add Participant.Parse implementation
			return false;
		}

		public bool Parse(object obj)
		{
			try
			{
				Participant participant = obj as Participant;
				this.from		 = participant.from;
				this.isModerator = participant.isModerator;
				this.isMuted     = participant.isMuted;
				this.isRecorded  = participant.isRecorded;
				this.lineId      = participant.lineId;
				this.callId      = participant.callId;
				this.timestamp   = participant.timestamp;
			}
			catch
			{
				return false;
			}

			return true;
		}

		#endregion
	}
}
