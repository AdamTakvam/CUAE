using System;
using System.Net;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Utilities.Collections;

namespace Metreos.MediaControl
{
    /// <summary>Collection of media resource group info</summary>
	/// <remarks>app name, partition name -> CacheData</remarks>
	internal class MrgCache
	{
        internal class CacheData
        {
            private readonly IPAddress[] mmsAddrs;
            public IPAddress[] MmsAddrs { get { return mmsAddrs; } }

            private readonly IPAddress[] failoverMmsAddrs;
            public IPAddress[] FailoverMmsAddrs { get { return failoverMmsAddrs; } }

            public CacheData(IPAddress[] mmsAddrs, IPAddress[] failoverMmsAddrs)
            {
                this.mmsAddrs = mmsAddrs;
                this.failoverMmsAddrs = failoverMmsAddrs;
            }
        }

        /// <summary>The cache colection</summary>
        /// <remarks>appName (string), partName (string) -> CacheData</summary>
        private readonly TwoKeyHash cache;

        private readonly LogWriter log;
        private readonly IConfigUtility configUtility;

        public CacheData this[string appName, string partName]
        {
            get { return GetCacheData(appName, partName, false); }
        }

        public MrgCache(LogWriter log, IConfigUtility configUtility)
		{
            this.log = log;
            this.configUtility = configUtility;

            this.cache = new TwoKeyHash();
		}

        public CacheData GetCacheData(string appName, string partName, bool diagServerSelection)
        {
            CacheData cData = cache[appName, partName] as CacheData;

            if(cData == null)
                cData = CacheCrgData(appName, partName, diagServerSelection);
            else if(diagServerSelection)
                log.Write(TraceLevel.Info, "Using cached MRG information for: {0}->{1}", appName, partName);

            return cData;
        }

        public void Clear()
        {
            this.cache.Clear();
        }

        #region Caching methods

        private CacheData CacheCrgData(string appName, string partName, bool diagServerSelection)
        {
            log.WriteIf(diagServerSelection, TraceLevel.Info, 
                "Fetching MRG infomation from database for: {0}->{1}", appName, partName);

            // Get primary MRG info
            ComponentInfo[] mediaComponents = configUtility.GetMediaResourceGroup(appName, partName);
            IPAddress[] mmsAddrs = GetMmsAddrs(mediaComponents);
            if(mmsAddrs == null) { return null; }

            // Get failover MRG info
            mediaComponents = configUtility.GetFailoverMRG(appName, partName);
            IPAddress[] failoverMmsAddrs = GetMmsAddrs(mediaComponents);

            CacheData cData = new CacheData(mmsAddrs, failoverMmsAddrs);
            this.cache.Add(appName, partName, cData);

            return cData;
        }

        private IPAddress[] GetMmsAddrs(ComponentInfo[] mediaComponents)
        {
            if(mediaComponents == null)
                return null;

            IPAddress[] mmsAddrs = new IPAddress[mediaComponents.Length];
            for(int i=0; i<mediaComponents.Length; i++)
            {
                ComponentInfo serverInfo = mediaComponents[i];
                
                IPAddress serverAddr = configUtility.GetEntryValue(
                    IConfig.ComponentType.MediaServer, serverInfo.name, IConfig.Entries.Names.ADDRESS) as IPAddress;
                if(serverAddr == null)
                {
                    log.Write(TraceLevel.Error, "No address configured for media server: " + serverInfo.name);
                    continue;
                }

                mmsAddrs[i] = serverAddr;
            }
            return mmsAddrs;
        }
        #endregion
	}
}
