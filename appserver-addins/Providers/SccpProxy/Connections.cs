using System.Net;
using System.Collections;
using System.Diagnostics;

using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents the set of TCP connections between localhost (us)
	/// and the CCMs and SCCP clients, although it makes no distinction
	/// between the two.
	/// </summary>
	/// <remarks>
	/// It is basically a thread-safe wrapper class around Hashtable.
	/// </remarks>
	public class Connections
	{
		/// <summary>
		/// Simple constructor for wrapper of hash table of connections.
		/// </summary>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		public Connections(SccpProxyProvider provider)
		{
			this.provider = provider;
			connections = ReportingDict.Wrap( "SccpProxy.Connections", Hashtable.Synchronized( new Hashtable() ) );
		}

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		private SccpProxyProvider provider;

		/// <summary>
		/// TCP connections with CCMs and SCCP clients--they are all in here.
		/// </summary>
		private IDictionary connections;

		/// <summary>
		/// Return connection object from the connections table with this
		/// IPEndPoint address.
		/// </summary>
		/// <remarks>
		/// If the address does not match one in the table, null is returned.
		/// </remarks>
		/// <param name="endPoint">Address of connection to return.</param>
		/// <value>Connection that has the indicated address, or null if not
		/// found.</value>
		public Connection this [IPEndPoint endPoint]
		{
			get { return (Connection) connections[endPoint]; }
		}

		/// <summary>
		/// Return connection object from the connections table over which
		/// this message was sent.
		/// </summary>
		/// <param name="message">Message sent over connection to return.</param>
		/// <value>Connection over which this message was sent to the proxy, or
		/// null if not found.</value>
		public Connection this [Message message]
		{
			get { return message.Connection; }
		}

		/// <summary>
		/// Remove connection from connections table.
		/// </summary>
		/// <param name="endPoint">The connection to remove.</param>
		public void Remove(Connection connection)
		{
			Assertion.Check(connection != null,
				"SccpProxyProvider: connection missing; cannot remove connection");
			
			Assertion.Check(connection.UniqueAddress != null,
				"SccpProxyProvider: connection does not have unique address");

			lock (connections.SyncRoot)
			{
				Connection c = (Connection) connections[connection.UniqueAddress];
				if (c == connection)
				{
					connections.Remove(connection.UniqueAddress);
					if (connection is ClientConnection)
						numConnections--;

					provider.LogWrite(TraceLevel.Info,
						"Cxs: removed {0}, {1} connections",
						connection, numConnections);
				}
			}
		}

		/// <summary>
		/// Returns whether the connection over which this message was received
		/// is still active.
		/// </summary>
		/// <param name="address">Message whose receive connection we are
		/// checking for activity.</param>
		/// <returns>Whether the connection is still active.</returns>
		public bool IsFromConnectionActive(Message message)
		{
			return IsConnectionActive(message.FromUniqueAddress);
		}

		/// <summary>
		/// Returns whether the connection over which this message is to be
		/// sent is still active.
		/// </summary>
		/// <param name="address">Message whose send connection we are
		/// checking for activity.</param>
		/// <returns>Whether the connection is still active.</returns>
		public bool IsToConnectionActive(Message message)
		{
			return IsConnectionActive(message.ToUniqueAddress);
		}

		/// <summary>
		/// Returns whether this connection is still active.
		/// </summary>
		/// <param name="address">Connection we are checking for activity.</param>
		/// <returns>Whether the connection is still active.</returns>
		public bool IsConnectionActive(Connection connection)
		{
			return IsConnectionActive(connection.UniqueAddress);
		}

		/// <summary>
		/// Returns whether the connection identified by this address is still
		/// active.
		/// </summary>
		/// <param name="address">Address whose connection we are checking for
		/// activity.</param>
		/// <returns>Whether the connection is still active.</returns>
		public bool IsConnectionActive(IPEndPoint address)
		{
			return address != null && connections.Contains(address);
		}

		/// <summary>
		/// Return whether there are fewer connections than the provided number.
		/// </summary>
		/// <param name="number">Number with which to compare.</param>
		/// <returns>Whether there are fewer connections.</returns>
		public bool IsRoomAvailable()
		{
			return numConnections < maxConnections;
		}

		private int numConnections;

		/// <summary>
		/// Method to invoke to report (or not) that no counterpart has been
		/// found for the connection over which the specified message was sent.
		/// </summary>
		/// <param name="message">Message sent over connection for which we are
		/// looking for counterpart.</param>
		public void ReportNoCounterpartOnProxy(Message message)
		{
			Connection connection = message.Connection;
			if (connection != null)
				connection.ReportNoCounterpartOnProxy(message);
		}

		/// <summary>
		/// Add connection to the connections table.
		/// </summary>
		/// <param name="connection">Connection to add to the table.</param>
		public void Add(Connection connection)
		{
			Assertion.Check(connection.UniqueAddress != null,
				"SccpProxyProvider: connection does not have unique address");

			lock (connections.SyncRoot)
			{
				connections.Add(connection.UniqueAddress, connection);
				
				if (connection is ClientConnection)
					numConnections++;

				provider.LogWrite(TraceLevel.Info,
					"Cxs: added {0}, {1} connections",
					connection, numConnections);
			}
		}

		public int MaxConnections
		{
			get { return maxConnections; }
			set { maxConnections = value; }
		}

		private int maxConnections;

		/// <summary>
		/// Remove and stop all connections.
		/// </summary>
		public void Clear()
		{
			IList xconnections;
			lock(connections.SyncRoot)
			{
				xconnections = new ArrayList( connections.Values );
			}

			foreach (Connection c in xconnections)
				c.Stop();
			
			connections.Clear();
			numConnections = 0;
		}

		/// <summary>
		/// Property that has the value of the number of connections in this
		/// wrapped collection.
		/// </summary>
		public int Count { get { return connections.Count; } }

		/// <summary>
		/// Log statistics about this object.
		/// </summary>
		public void LogStatistics()
		{
			provider.LogWrite(TraceLevel.Info,
				"Cxs: {0} connections",
				connections.Count);
		}
	}
}
