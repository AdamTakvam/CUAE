using System;
using System.Net;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using Metreos.LoggingFramework;
using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy.RtpRelay
{
	/// <summary>Manages connections to RTP relay servers</summary>
	public class RelayManager
	{
        public delegate void RelayDisconnectedDelegate(RelayConnection conn);

        // Queue for outbound messages to the RTP relays
        private Queue msgQueue = new Queue();

        // Used for load-balancing and failover
        // ConnectionId (int) -> RelayConnection (object)

        private bool shuttingDown = false;
        private Thread messageThread;
        private LogWriter log;

        public RelayDisconnectedDelegate OnRelayDisconnected;

		public RelayManager(LogWriter log)
		{
            this.log = log;

            this.messageThread = new Thread(new ThreadStart(MessageThread));
            this.messageThread.Name = "RTP Relay Manager Message Thread";
            this.messageThread.IsBackground = true;

			connections = ReportingDict.Wrap( "RelayManager.connections", Hashtable.Synchronized(new Hashtable()) );
			pendingConnections = ReportingDict.Wrap( "RelayManager.pendingConnections", Hashtable.Synchronized(new Hashtable()) );
		}

		#region Relay Connection Management

		private void AddConnection( RelayConnection conn )
		{
			connections.Add( conn.ConnectionId, conn );
		}

		private RelayConnection GetConnection( int connectionId )
		{
			return (RelayConnection) connections[connectionId];
		}

		private IList GetConnections( bool clear )
		{
			lock (connections.SyncRoot)
			{
				IList list = new ArrayList( connections.Values );
				if (clear)
					connections.Clear();
				return list;
			}
		}

		private bool RemoveConnection( RelayConnection conn )
		{
			lock (connections.SyncRoot)
			{
				if (connections.Contains( conn.ConnectionId ))
				{
					connections.Remove( conn.ConnectionId );
					return true;
				}
				return false;
			}
		}

		private RelayConnection RemoveConnection( int connectionId )
		{
			lock (connections.SyncRoot)
			{
				RelayConnection conn = GetConnection( connectionId );
				if (conn != null)
					connections.Remove( connectionId );
				return conn;
			}
		}
		
		private IDictionary connections;

		#endregion
		#region Pending Relay Connection Management

		private void AddPendingConnection( RelayConnection conn )
		{
			pendingConnections.Add( conn.ConnectionId, conn );
		}

		private RelayConnection GetPendingConnection( int connectionId )
		{
			return (RelayConnection) pendingConnections[connectionId];
		}

		private IList GetPendingConnections( bool clear )
		{
			lock (pendingConnections.SyncRoot)
			{
				IList list = new ArrayList( pendingConnections.Values );
				if (clear)
					pendingConnections.Clear();
				return list;
			}
		}

		private bool RemovePendingConnection( RelayConnection conn )
		{
			lock (pendingConnections.SyncRoot)
			{
				if (pendingConnections.Contains( conn.ConnectionId ))
				{
					pendingConnections.Remove( conn.ConnectionId );
					return true;
				}
				return false;
			}
		}

		private RelayConnection RemovePendingConnection( int connectionId )
		{
			lock (pendingConnections.SyncRoot)
			{
				RelayConnection conn = GetPendingConnection( connectionId );
				if (conn != null)
					pendingConnections.Remove( connectionId );
				return conn;
			}
		}

		private IDictionary pendingConnections;
		
		#endregion
        #region Config refresh & Shutdown

        public void ClearConfirmationFlags()
        {
            foreach(RelayConnection conn in GetConnections(false))
				conn.Confirmed = false;
        }

        public bool Confirm(IPAddress relayDmzAddr)
        {
            // Search by value is not optimal
            //  but it's only called by RefreshConfiguration()
            foreach(RelayConnection conn in GetConnections(false))
            {
                if(conn.RelayAddress.Equals(relayDmzAddr))
                {
                    conn.Confirmed = true;
                    return true;
                }
            }
            return false;
        }

        public void CloseUnconfirmedConnections()
        {
			foreach(RelayConnection conn in GetConnections(false))
			{
				if(conn.Confirmed == false)
				{
					RemoveConnection(conn);
					conn.Close();
				}
			}
		}

		public void Startup()
		{
			if(!messageThread.IsAlive)
			{
				messageThread.Start();

				foreach(RelayConnection conn in GetPendingConnections(false))
					conn.Connect();
			}
		}

        public void Shutdown()
		{
			shuttingDown = true;

			foreach(RelayConnection conn in GetPendingConnections( true ))
				conn.Close();

			foreach(RelayConnection conn in GetConnections( true ))
				conn.Close();

			KillMessageThread();
		}

        #endregion

        #region Relay server management

        public RelayConnection AddRtpRelayServer(IPEndPoint ipcAddr)
        {
            int connectionId = GetNextConnectionId();
            RelayConnection conn = new RelayConnection(connectionId, log, ipcAddr, this);
            conn.OnConnected = new ConnectionStatusDelegate(OnConnected);
            conn.OnDisconnected = new ConnectionStatusDelegate(OnDisconnected);

			AddPendingConnection(conn);

			if (messageThread.IsAlive)
			{
				log.Write(TraceLevel.Info, "Connecting to RTP relay: {0} ({1})", connectionId, ipcAddr);
				conn.Connect();
			}

			return conn;
        }

		private int GetNextConnectionId()
		{
			lock (this)
			{
				return nextConnectionId++;
			}
		}

		private int nextConnectionId = 1;

        private void OnConnected(RelayConnection conn)
		{
			lock (this)
			{
				RemovePendingConnection( conn );
				AddConnection( conn );
			}
        }
        
        private void OnDisconnected(RelayConnection conn)
        {
			lock (this)
			{
				if(!RemoveConnection( conn ))
					return;
			
				if(!shuttingDown)
				{
					// Throw it back in the pending pool and wait til the server comes back up
					AddPendingConnection( conn );
					OnRelayDisconnected(conn);
				}
			}
        }
        #endregion

        #region Relay channel management

        public Relay CreateRtpRelay(IPEndPoint remoteEP, int addrIndice)
        {
            return CreateRtpRelay(remoteEP, addrIndice, SortByNumRelays(GetConnections(false)));
        }

		/// <summary>
		/// Creates a list of relay connections sorted ascending by num relays.
		/// </summary>
		/// <param name="conns">a list of relay connections</param>
		/// <returns>a list of relay connections sorted ascending by num relays</returns>
		private RelayConnection[] SortByNumRelays( IList conns )
		{
			RelayConnection[] sortedConns = new RelayConnection[conns.Count];
			int[] keys = new int[conns.Count];

			int i = 0;
			foreach (RelayConnection conn in conns)
			{
				sortedConns[i] = conn;
				keys[i] = conn.Desireability;
				i++;
			}

			Array.Sort( keys, sortedConns );
			return sortedConns;
		}

		/// <summary>
		/// Creates a relay from the list of available connections by trying them
		/// each in turn until one works.
		/// </summary>
		/// <param name="remoteEP"></param>
		/// <param name="addrIndice"></param>
		/// <param name="conns"></param>
		/// <returns>the created relay or null</returns>
        private Relay CreateRtpRelay(IPEndPoint remoteEP, int addrIndice, RelayConnection[] conns)
        {
			foreach (RelayConnection conn in conns)
			{
				log.Write(TraceLevel.Info, "trying to create relay to {0} using {1}",
					remoteEP, conn.ConnectionId);
				Relay relay = conn.CreateRelay(remoteEP, addrIndice);
				if (relay != null)
				{
					return relay;
				}
			}
			return null;
        }

        #endregion
        #region Message Thread

		public void MsgQueueEnqueue( RelayMsg msg )
		{
			lock (msgQueue)
			{
				if (shuttingDown)
					throw new InvalidOperationException( "shuttingDown" );

				msgQueue.Enqueue( msg );
				
				if (msgQueue.Count == 1)
					Monitor.Pulse( msgQueue );
			}
		}

		private void KillMessageThread()
		{
			lock (msgQueue)
			{
				Monitor.Pulse( msgQueue );
			}

			if (messageThread != null)
			{
				if (!messageThread.Join( 5000 ))
				{
					messageThread.Abort();
					if (!messageThread.Join( 1000 ))
					{
						messageThread.Interrupt();
						messageThread.Join( 1000 );
					}
				}
				messageThread = null;
			}

			lock (msgQueue)
			{
				msgQueue.Clear();
			}
		}

        private void MessageThread()
        {
            // Read from the queue and forward the message to the proper connection
            while(!shuttingDown)
            {
				RelayMsg msg;

				lock (msgQueue)
				{
					while (!shuttingDown && msgQueue.Count == 0)
						Monitor.Wait( msgQueue );
					
					if (shuttingDown)
						continue;

					msg = (RelayMsg) msgQueue.Dequeue();
				}

                RelayConnection conn = GetConnection( msg.ConnectionId );
                if(conn == null)
                {
                    log.Write(TraceLevel.Warning, "Attempted to send message to unknown relay '{0}':\n{1}",
                        msg.ConnectionId, msg.ToString());
                    continue;
                }

                conn.SendMessage(msg);
            }

			lock (msgQueue)
			{
				msgQueue.Clear();
			}
        }
        #endregion
	}
}
