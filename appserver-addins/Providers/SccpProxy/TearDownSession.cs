using System;
using System.Threading;
using System.Diagnostics;
using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class encapsulates a short-lived thread that tears down a session.
	/// </summary>
	public class TearDownSession
	{
		/// <summary>
		/// Constructor for thread to tear down session.
		/// </summary>
		/// <param name="timeoutMs">Number of milliseconds to wait for the
		/// writer lock.</param>
		/// <param name="sessions">List of all sessions.</param>
		/// <param name="sid">"SCCP IDentifier," a.k.a., device name.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		public TearDownSession(int timeoutMs, Sessions sessions, Session session,
			SccpProxyProvider provider)
		{
			Assertion.Check(timeoutMs > 0, "SccpProxyProvider: non-positive timeout");
			Assertion.Check(sessions != null, "SccpProxyProvider: missing sessions");
			Assertion.Check(session != null, "SccpProxyProvider: missing session");
			Assertion.Check(provider != null, "SccpProxyProvider: missing provider");

			// Set this so nobody starts accessing the session. Once set
			// to true, it is never reset to false.
			session.NeedToTearDown = true;

			this.timeoutMs = timeoutMs;
			this.sessions = sessions;
			this.session = session;
			this.provider = provider;

			// Pulse pending message so we won't wait for action from app.
			PulsePendingMessage(session);

			// NOTE: get the thread pool to handle this.
			provider.tp.PostRequest( new WorkRequestDelegate( TearDownSessionThread ) );
		}

		/// <summary>
		/// If there is a pending message, remove it from the session and Pulse
		/// it in case there is a Wait on it.
		/// </summary>
		/// <param name="session"></param>
		private void PulsePendingMessage(Session session)
		{
			if (session.HasPendingMessage)
			{
				Message message = session.RemovePendingMessage();
				if (message != null)	// Paranoia--make sure message present
				{
					lock (message)
					{
						Monitor.Pulse(message);
					}
				}
			}
		}

		/// <summary>
		/// Session to tear down.
		/// </summary>
		private Session session;

		/// <summary>
		/// Number of milliseconds to wait for the writer lock.
		/// </summary>
		private int timeoutMs;

		/// <summary>
		/// List of sessions between client and CCM.
		/// </summary>
		private Sessions sessions;

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		protected SccpProxyProvider provider;

		/// <summary>
		/// Thread entry point that waits on writer lock before tearing down
		/// session.
		/// </summary>
		private void TearDownSessionThread( object state )
		{
			Assertion.Check(session != null, "SccpProxyProvider: missing session");
			Assertion.Check(sessions != null, "SccpProxyProvider: missing sessions");

			try
			{
				ReaderWriterLock needToTearDownLock = session.NeedToTearDownLock;
				if (needToTearDownLock != null)	// Just paranoia.
				{
					// Pulse pending message again so we won't wait for action
					// from app.
					PulsePendingMessage(session);

					needToTearDownLock.AcquireWriterLock(timeoutMs);
					string sessionName = session.ToString();
					try
					{
						sessions.TearDownSession(session.Sid);
					}
					finally
					{
						needToTearDownLock.ReleaseWriterLock();
					}
				}
				else
				{
					provider.LogWrite(TraceLevel.Error,
						"Ter: no lock to acquire for {0}", session);
				}
			}
			catch(Exception e)
			{
				provider.LogWrite(TraceLevel.Error, "Ter: {0}", e);
			}
		}
	}
}
