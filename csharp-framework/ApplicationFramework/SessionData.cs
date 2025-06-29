using System;
using System.Net;
using System.Data;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Utilities;
using Metreos.ApplicationFramework.Collections;

namespace Metreos.ApplicationFramework
{
	[Serializable]
	public sealed class SessionData
	{
        /// <summary>Hashtable for freeform application use</summary>
        public Hashtable CustomData { get { return customData; } }
        private readonly Hashtable customData;

        /// <summary>Collection of all database connections for this application</summary>
        public AdoDbCollection DbConnections { get { return dbConnections; } }
        private readonly AdoDbCollection dbConnections;

        /// <summary>The name of this application</summary>
        public string AppName { get { return appName; } }
        private readonly string appName;

        /// <summary>The partition which is in effect for this instance</summary>
        public string PartitionName { get { return partitionName; } }
        private readonly string partitionName;

        /// <summary>The current culture (locale)</summary>
        public CultureInfo Culture { get { return culture; } }
        private CultureInfo culture;

        /// <summary>The ID of this instance</summary>
        /// <remarks>This ID is only unique within this application. It is not a GUID.</remarks>
        public long InstanceId { get { return instanceId; } }
        private readonly long instanceId;

        /// <summary>IP addresses of all local interfaces</summary>
        public IPAddress[] LocalInterfaces { get { return IpUtility.GetIPAddresses(); } }

		public SessionData(string appName, string partitionName, CultureInfo culture, long instanceId)
		{
            this.appName = appName;
            this.partitionName = partitionName;
            this.culture = culture;
            this.instanceId = instanceId;

            this.customData = new Hashtable();
            this.dbConnections = new AdoDbCollection();
		}

        public void Clear()
        {
            dbConnections.Clear();
            customData.Clear();
        }

        public bool ChangeCulture(string newCulture)
        {
            if (newCulture != null && 
                newCulture != String.Empty)
            {
                try
                {
                    this.culture = new System.Globalization.CultureInfo(newCulture);
                    return true;
                }
                catch { /* Do nothing */ }
            }

            return false;
        }
	}
}
