using System;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for IHttpCallbacks.
	/// </summary>
	public interface IHttpCallbacks
	{
		void CreateConferenceFailure(string locationGuid);

        void CreateConferenceSuccess(string locationGuid, string locationId, string  remotePartyNumber);

        void JoinConferenceFailure(string locationGuid);

        void JoinConferenceSuccess(string locationGuid, string locationId, string remotePartyNumber);

        void KickConferenceFailure(string locationGuid, string locationId);

        void KickConferenceSuccess(string locationGuid, string locationId);

        void MuteConnectionFailure(string locationGuid, string locationId);

        void MuteConnectionSuccess(string locationGuid, string locationId);

        void Join404(string locationGuid);

        void Kick404(string locationGuid);

        void Mute404(string locationGuid);

        void CheckCreate404();

        void CheckJoin404();

        void CheckKick404();
        
        void AssignSessionId(string sessionId);

        void AssignConferenceId(string conferenceId);

        void CheckConferenceDatabase(string sessionId, string locationGuid, string locationId, string remotePartyNumber);

        void CheckJoinDatabase(string sessionId, string locationGuid, string locationId, string remotePartyNumber);

        void CheckKickDatabase(string sessionId, string locationGuid, string locationId);
	}
}
