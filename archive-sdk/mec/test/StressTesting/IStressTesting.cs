using System;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for IStressTesting.
	/// </summary>
	public abstract class IStressTesting
	{
        public const string KICKING = "leaving";

        public const string MUTING = "muting";

        public const string UNMUTING = "unmuting";

        public const string MUTED = "muted";

		public delegate void CreateConferenceDelegate(string phoneNumber, bool allowRandom);

		public delegate void EndConferenceDelegate(string conferenceId);

		public delegate void JoinLocationDelegate(string conferenceId, string phoneNumber);

		public delegate void KickLocationDelegate(string locationGuid, string conferenceId, string locationId);

		public delegate void MuteLocationDelegate(string locationGuid, string conferenceId, string locationId);

        public delegate void TerminateAllDelegate();
	}
}
