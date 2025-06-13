using System;
using System.Net;
using System.Collections;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;

namespace Metreos.Providers.CiscoDeviceListX
{
	internal abstract class DeviceListXReader
	{
        // DLX LogWriter 
        protected readonly LogWriter log;
        
        // cluster that the reader is servicing
		protected readonly CallManagerCluster cluster;
        public CallManagerCluster Cluster { get { return cluster; } }
		
        // DeviceList object that contains device information
        protected DeviceList deviceList;

        internal string ClusterIP { get { return cluster.PublisherIP.ToString(); } }
        internal DeviceList Data  { get { return this.deviceList;     } }

		// default cluster constructor
		public DeviceListXReader(LogWriter log, CallManagerCluster cluster) //: this(log)
		{
            this.log = log;
            this.cluster = cluster;
            this.deviceList = new DeviceList();
		}

        /// <summary>
        /// When invoked on a DLX reader object, the reader will attempt to retrieve device information
        /// from the CallManager.
        /// </summary>
        /// <returns>true if retrieval succeeded, false otherwise</returns>
		internal abstract bool RetrieveDeviceList(ref bool shutdownFlag);

        /// <summary>
        /// Initializes the necessary fields that belong to the reader object and validates
        /// that the reader object has all the information it needs in order to do its job.
        /// </summary>
        /// <returns>true if the object was initialized properly, false otherwise</returns>
        internal abstract bool Initialize();
	}
}
