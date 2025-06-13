using System;
using System.Net;
using System.Collections;
using System.Diagnostics;
using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents the collection of active SCCP sessions within
	/// the proxy.
	/// </summary>
	public class Sessions : IDisposable, IEnumerable
	{
		/// <summary>Simple constructor for a collection of SCCP sessions.</summary>
		/// <param name="connections">List of CCM and client connections.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		public Sessions(Connections connections, SccpProxyProvider provider)
		{
			Assertion.Check(connections != null,
				"SccpProxyProvider: connections cannot be null");
			Assertion.Check(provider != null,
				"SccpProxyProvider: provider cannot be null");

			this.connections = connections;
			this.provider = provider;
			this.sessions = ReportingDict.Wrap( "Sessions.sessions", Hashtable.Synchronized(new Hashtable()) );
		}

		/// <summary>Constants</summary>
		private abstract class Consts
		{
			/// <summary>
			/// Maximum number of sessions represented by the
			/// Session.ToString() method.
			/// </summary>
			/// <remarks>
			/// This is limited so that ToString() doesn't end up returning a
			/// string that is extremely large.
			/// </remarks>
			public const int SessionsInToStringMax = 5;
		}

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		private SccpProxyProvider provider;

		/// <summary>TCP connections with CCMs and SCCP clients.</summary>
		private readonly Connections connections;

		/// <summary>Collection of SCCP session objects.</summary>
		private readonly IDictionary sessions;

		/// <summary>Dispose sessions.</summary>
		public void Dispose()
		{
			foreach (Session s in GetSessions())
				s.Dispose();

			sessions.Clear();
		}

		/// <summary>
		/// Destroy sessions.
		/// </summary>
		/// <remarks>
		/// This destructor runs only if the Dispose method does not get
		/// called. It gives the base class the opportunity to finalize.
		/// </remarks>
		~Sessions()
		{
			Dispose();
		}

		/// <summary>
		/// Return session object from the sessions table belonging to the
		/// device with the specified Station IDentifier.
		/// </summary>
		/// <param name="sid">Station IDentifier that uniquely identifies the
		/// session.</param>
		/// <value>Session belonging to this Station IDentifier.</value>
		public Session this[string sid]
		{
			get
			{
				lock (sessions.SyncRoot)
				{
					return (Session) sessions[sid];
				}
			}
		}

		/// <summary>
		/// Indexer whose value is the session from the sessions list within
		/// which the specified message was sent.
		/// </summary>
		public Session this [Message message] { get { return message.Connection.MySession; } }

		/// <summary>
		/// Indexer whose value is the session from the sessions list that
		/// is identified by the specified connection address.
		/// </summary>
		public Session this [IPEndPoint address]
		{
			get
			{
				Connection connection = connections[address];
				if (connection != null)
					return connection.MySession;

				return null;
			}
		}

		/// <summary>
		/// Add a pending message to the session on which this message was received.
		/// </summary>
		/// <param name="message">Message to add to the session as a pending
		/// message.</param>
		public void AddPendingMessage(Message message)
		{
			Connection connection = message.Connection;
			if (connection != null)
			{
				Session session = connection.MySession;
				if (session != null)
				{
					session.AddPendingMessage(message);
				}
			}
			else
			{
				provider.LogWrite(TraceLevel.Error,
					"Sss: {0} not associated with connection; not added as pending message",
					message);
			}
		}

		/// <summary>
		/// Remove and return the pending message for the session that has the
		/// specified sid.
		/// </summary>
		/// <param name="sid">Station IDentifier that uniquely identifies the
		/// session.</param>
		/// <returns>Pending message.</returns>
		public Message RemovePendingMessage(string sid)
		{
			Session session = this[sid];
			return session != null ? session.RemovePendingMessage() : null;
		}

		/// <summary>
		/// Add session to list, identified by unique Station IDentifier.
		/// </summary>
		/// <param name="connection">SID that unqiue address identifies
		/// the session.</param>
		/// <param name="session">Session to add to list.</param>
		public bool Add(Session session)
		{
			lock (sessions.SyncRoot)
			{
				if(sessions.Contains(session.Sid))
					return false;

				sessions.Add(session.Sid, session);

				provider.LogWrite(TraceLevel.Info,
					"added {0}, count = {1}", session.Sid, sessions.Count);
			}

			return true;
		}

		public Session Remove(Message message)
		{
			Connection connection = message.Connection;

			if (connection == null)
				Assertion.Check(connection != null,
					"SccpProxyProvider: " + message.ToString() + " not associated with connection");

            return Remove(connection);
		}

		public Session Remove(Connection connection)
		{
			Assertion.Check(connection != null, "SccpProxyProvider: missing connection");
		    Assertion.Check(connection.MySession != null, "SccpProxyProvider: connection not associated with session");

			return Remove(connection.MySession.Sid);
		}

		/// <summary>
		/// Remove session from list (identified with the specified Station
		/// IDentifier) and return it.
		/// </summary>
		/// <param name="sid">Station IDentifier that uniquely identifies the
		/// session to remove from the list.</param>
		/// <returns>Session identified by the Station IDentifier.</returns>
		public Session Remove(string sid)
		{
			lock (sessions.SyncRoot)
			{
				Session session = this[sid];
				if (session != null)
				{
					sessions.Remove(sid);
					provider.LogWrite(TraceLevel.Info,
						"removed {0}, count = {1}", sid, sessions.Count);
				}
				return session;
			}
		}

		/// <summary>
		/// Tear down the session with this Station IDentifier.
		/// </summary>
		/// <param name="sid">Station IDentifier whose session to tear
		/// down.</param>
		public void TearDownSession(string sid)
		{
			Session session = Remove(sid);
			if (session != null)
			{
				session.Dispose();
			}
		}

		/// <summary>
		/// Returns a string representation of the sessions hashtable.
		/// </summary>
		/// <remarks>
		/// Only the first few sessions are represnted. If there are more that
		/// are not, "..." is appended to string.
		/// </remarks>
		/// <returns>String representation of the sessions hashtable.</returns>
		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			int i = 0;
            IEnumerator e = GetSessions().GetEnumerator();
			while (e.MoveNext() && i++ < Consts.SessionsInToStringMax)
			{
				if (i > 0)
					sb.Append(" ");
				sb.Append(((Session)e.Current).ToString());
			}

			// Append "..." if there are more sessions than we are representing
			// in the return string.
			if (i > Consts.SessionsInToStringMax)
				sb.Append("...");

			return sb.ToString();
		}

		/// <summary>
		/// Property that has the value of the number of sessions in this
		/// wrapped collection.
		/// </summary>
		public int Count { get { return sessions.Count; } }

		/// <summary>Log statistics about this object.</summary>
		public void LogStatistics()
		{
			int rtpVoiceRelayTotal = 0;
            int rtpVideoRelayTotal = 0;
			int subscriptionTotal = 0;
			foreach (Session session in GetSessions())
			{
                if(session.RtpVoiceRelay != null)
					rtpVoiceRelayTotal += 1;
                
				if(session.RtpVideoRelay != null)
                    rtpVideoRelayTotal += 1;

				subscriptionTotal += session.SubscriptionsCount;
			}
			
			provider.LogWrite(TraceLevel.Info,
				"Sss: sessions: {0}, rtpVoiceRelays: {1}, rtpVideoRelays: {2}, subscriptions: {3}",
				Count, rtpVoiceRelayTotal, rtpVideoRelayTotal, subscriptionTotal);
        }

		private IList GetSessions()
		{
			lock (sessions.SyncRoot)
			{
				return new ArrayList( sessions.Values );
			}
		}

        #region IEnumerable Members

        public object SyncRoot { get { return this.sessions.SyncRoot; } }

        public IEnumerator GetEnumerator()
        {
			lock (sessions.SyncRoot)
			{
				return new ArrayList( sessions.Values ).GetEnumerator();
			}
        }

        #endregion
    }
}
