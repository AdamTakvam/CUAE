using System;

namespace Metreos.Mec.StressApp
{
	/// <summary>
	/// Summary description for IConferenceCallbacks.
	/// </summary>
	public interface IConferenceCallbacks
	{
        int QueryPendingJoins();

        int QueryPendingCreates();

        int QueryPendingKicks();

        int QueryLiveConnections();

		void UpdatePendingJoins(int newValue);

        void UpdatePendingCreates(int newValue);

        void UpdatePendingKicks(int newValue);

        void UpdateLiveConnections(int newValue);

        void UpdateLiveConferences(int newValue);

        void RemoveMe(Conference me);

        bool IsFreeLocation();

        bool IsFreeConference();

        void ReturnCheckOutLocation();

        void ReturnCheckOutConference();
	}
}
