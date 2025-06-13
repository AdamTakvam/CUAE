using System;
using System.Net;
using System.Collections;
using System.Diagnostics;

namespace Metreos.Providers.SccpProxy
{
	/// <summary>
	/// This class represents "registrations" of CCMs and clients with the app
	/// and _not_ client registrations with CCMs.
	/// </summary>
	public class Registrations
	{
		/// <summary>
		/// Simple constructor.
		/// </summary>
		/// <param name="connections">TCP connections to CCMs and clients.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		public Registrations(Connections connections, SccpProxyProvider provider)
		{
			this.connections = connections;
			this.provider = provider;

			registrations = new Hashtable();
		}

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		SccpProxyProvider provider;

		/// <summary>
		/// Hashtable to map from SCCP client address to GUID.
		/// </summary>
		private Hashtable registrations;

		/// <summary>
		/// TCP connections with CCMs and SCCP clients--they are all in here.
		/// </summary>
		private Connections connections;

		/// <summary>
		/// Return GUID from the registrations table with this
		/// IP address/port number.
		/// </summary>
		/// <remarks>
		/// If the address does not match one in the table, null is returned.
		/// </remarks>
		/// <param name="endPoint">IP Address/port number of GUID to
		/// return.</param>
		/// <value>GUID for the indicated address, or null if not
		/// found.</returns>
		public string this [string endPoint]
		{
			get 
			{
				string guid;

				lock (this)
				{
					guid = (string)registrations[endPoint];
				}

				return guid;
			}
		}

		/// <summary>
		/// Clear our simple hashtable that represents registrations.
		/// </summary>
		public void Clear()
		{
			lock (this)
			{
				registrations.Clear();
			}
		}

		/// <summary>
		/// Remove the entry in the registration table for this address.
		/// </summary>
		/// <param name="address">Address of the registration to tear down.</param>
		public void Remove(IPEndPoint address)
		{
			lock (this)
			{
				registrations.Remove(address.ToString());
			}
		}

		/// <summary>
		/// Use existing GUID if this SCCP client is already registered;
		/// otherwise, create a new one and add it to the table.
		/// </summary>
		/// <param name="message">Any message from the client.</param>
		/// <returns>GUID for the client.</returns>
		public string NeedGuidForClient(Message message)
		{
			string clientAddress = message.FromRemote.ToString();
			string routingGuid;
			lock (this)
			{
				if (registrations.Contains(clientAddress))
				{
					routingGuid = (string)registrations[clientAddress];
				}
				else
				{
					routingGuid = System.Guid.NewGuid().ToString();
					registrations.Add(clientAddress, routingGuid);
				}
			}

			return routingGuid;
		}


		/// <summary>
		/// Using the same GUID for the SCCP client, create a new registration
		/// entry for the CCM port and add it to the table.
		/// </summary>
		/// <remarks>
		/// This way, we are able to translate from an address to a GUID
		/// regardless of where the message came from (CCM or client).
		/// </remarks>
		/// <param name="message">Any message from the client.</param>
		/// <returns>GUID for the CCM.</returns>
		public string NeedGuidForCCM(Message message)
		{
			Debug.Assert(message.ToRemote != null, "Missing to address");
			Debug.Assert(message.FromRemote != null, "Missing from address");

			string routingGuid;
			lock (this)
			{
				// Use the client's GUID
				routingGuid = (string)registrations[message.FromRemote.ToString()];
				if (routingGuid != null)
				{
					if (registrations.Contains(message.ToLocal.ToString()))
					{
						Debug.Fail("proxy address to CCM, " + message.ToLocal.ToString()+
							", already registered with GUID, " +
							(string)registrations[message.ToLocal.ToString()]);

						// There really should not have been a GUID for the CCM. To
						// keep going, remove it so we can add a new, "good" one.
						registrations.Remove(message.ToLocal.ToString());
					}
					registrations.Add(message.ToLocal.ToString(), routingGuid);
				}
			}

			return routingGuid;
		}
	}
}
