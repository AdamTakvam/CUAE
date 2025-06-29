using System;

using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;

namespace Metreos.AppServer.TelephonyManager
{
	/// <summary>App partition config data</summary>
	public class CrgData
	{
        /// <summary>App partition config settings</summary>
        public AppPartitionInfo PartitionInfo { get { return pInfo; } }
        private readonly AppPartitionInfo pInfo;

        /// <summary>The set of devices in this partition's call route group</summary>
        public CrgMember[] Members { get { return members; } }
        private readonly CrgMember[] members;

		public CrgData(AppPartitionInfo pInfo, CrgMember[] members)
		{
            this.pInfo = pInfo;
            this.members = members;
		}
	}
}
