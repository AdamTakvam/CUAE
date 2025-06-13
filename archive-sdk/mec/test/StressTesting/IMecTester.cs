using System;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for IMecTester.
	/// </summary>
	public interface IMecTester
	{
        // conference view information

		void NewConference(string conferenceGuid, string locationGuid, string status, string phoneNumber);

        void NewLocation(string conferenceGuid, string locationGuid, string conferenceId, string status, string phoneNumber);

        void UpdateLocation(string conferenceGuid, string locationGuid, string conferenceId, string locationId, string status, string phoneNumber);

        void UpdateEstablishedLocation(string conferenceGuid, string locationGuid, string conferenceId, string locationid, string status);

        void RemoveLocation(string conferenceGuid, string locationGuid);

        void RemoveConference(string conferenceGuid);

        void UpdateConference(string conferenceGuid, int liveConnections, int pendingJoins, string status, int pendingKicks);


        // runtime statistics

        void UpdatePendingConferences(int pendingConferences);

        void UpdateLiveConferences(int liveConferences);

        void UpdateTotalConferences(int totalConferences);

        void UpdateFailedConferences(int failedConferences);

        void UpdatePendingConnections(int pendingConnections);

        void UpdateLiveConnections(int liveConnections);

        void UpdateTotalConnections(int totalConnections);

        void UpdateFailedConnections(int failedConnections);

        void UpdatePendingKicks(int pendingKicks);

        void UpdateTotalKicks(int totalKicks);

        void UpdateFailedKicks(int failedKicks);


        // logging information

        void UpdateVerboseInfo(string info);

        void UpdateErrorInfo(string info);
        

		// Viewer generated callbacks
        // Register mechanism for callbacks
        void RegisterCreateConference(IStressTesting.CreateConferenceDelegate createConferenceDelegate);

        void RegisterEndConference(IStressTesting.EndConferenceDelegate endConferenceDelegate);

        void RegisterJoinLocation(IStressTesting.JoinLocationDelegate joinLocationDelegate);

        void RegisterKickLocation(IStressTesting.KickLocationDelegate kickLocationDelegate);

        void RegisterMuteLocation(IStressTesting.MuteLocationDelegate muteLocationDelegate);

        void RegisterTerminateAll(IStressTesting.TerminateAllDelegate terminateAllDelegate);

        // Trigger Callbacks
 
        void CreateConference(string phoneNumber, bool allowRandom);

        void EndConference(string conferenceId);

        // -1 means use auto generated number
        void JoinLocation(string conferenceId, string phoneNumber);

        void KickLocation(string locationGuid, string conferenceId, string locationId);

        void MuteLocation(string locationGuid, string conferenceId, string locationId);

        void TerminateAll();

	}
}
